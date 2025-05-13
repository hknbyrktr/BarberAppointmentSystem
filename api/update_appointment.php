<?php

$conn = new mysqli("localhost", "root", "", "berberrandevusistemi");                            // Veritabani baglantisi.
$conn->set_charset("utf8");

if ($conn->connect_error) {                                                                     // Hata kontrolu.
    die("Bağlantı hatası: " . $conn->connect_error);
}

$barberID = $_POST["barberID"];                                                                 // Unity'den gelen POST verileri
$userName = $_POST["userName"];
$appointmentDate = $_POST["appointmentDate"];
$appointmentTime = $_POST["appointmentTime"];
$registrationDate = $_POST["registrationDate"];
$services = $_POST["services"];


$conn->begin_transaction();

try {

    $stmt0 = $conn->prepare("SELECT customerID FROM musteri WHERE userName = ?");              // Kullanici adindan customerID'yi al.
    $stmt0->bind_param("s", $userName);
    $stmt0->execute();
    $result = $stmt0->get_result();

    if ($result->num_rows === 0) {
        throw new Exception("Kullanıcı bulunamadı.");
    }

    $row = $result->fetch_assoc();
    $customerID = $row["customerID"];
    $stmt0->close();

    $stmt = $conn->prepare("SELECT appointmentID FROM randevu WHERE barberID = ? AND appointmentDate = ? AND appointmentTime = ?");         // Ilgili satiri bul.
    $stmt->bind_param("iss", $barberID, $appointmentDate, $appointmentTime);
    $stmt->execute();
    $result = $stmt->get_result();

    $appointmentID = null;

    if ($row = $result->fetch_assoc()) {
        $appointmentID = $row['appointmentID'];

        $stmt_update = $conn->prepare("UPDATE randevu SET customerID = ?, registrationDate = ? WHERE appointmentID = ?");                   // Guncelleme islemi
        $stmt_update->bind_param("isi", $customerID, $registrationDate, $appointmentID);
        $stmt_update->execute();
        $stmt_update->close();
    } else {
        echo "Belirtilen tarihe ve saate sahip bir randevu bulunamadı.";
       
    }

    $serviceIDs = explode(",", $services);                                                                                                 // Hizmetleri randevuya bagla.
    $stmt2 = $conn->prepare("INSERT INTO randevuhizmet (appointmentID, serviceID) VALUES (?, ?)");

    foreach ($serviceIDs as $serviceID) {
        $serviceID = intval($serviceID);
        $stmt2->bind_param("ii", $appointmentID, $serviceID);
        $stmt2->execute();
    }
    $stmt2->close();

    $conn->commit();
    echo "Success";

} catch (Exception $e) {
    $conn->rollback();
    echo "Error: " . $e->getMessage();
}

$conn->close();
?>
