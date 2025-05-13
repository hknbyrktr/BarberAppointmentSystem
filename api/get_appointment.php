<?php


$conn = new mysqli("localhost", "root", "", "berberrandevusistemi");                         // Veritabani baglantisi
$conn->set_charset("utf8");


if ($conn->connect_error) {                                                                  // Hata kontrolu.
    die("Baglanti hatasi: " . $conn->connect_error);
}

if (!isset($_GET['userName'])) {
    echo "userName parametresi eksik. Lütfen çıkış yapıp tekrar bilgilerinizi doğrulayın";
    exit();
}

$userName = $_GET['userName'];

$stmt = $conn->prepare("SELECT customerID FROM musteri WHERE userName = ?");                 // Musterinin ID'sini al
$stmt->bind_param("s", $userName);
$stmt->execute();
$result = $stmt->get_result();


if ($result->num_rows === 0) {
    echo "userNotExist";
    exit;
}

                                                                                            // Randevu ve berber bilgilerini al
$customerRow = $result->fetch_assoc();
$customerID = $customerRow['customerID'];

$stmt = $conn->prepare("                                                                  
    SELECT 
        r.barberID, r.appointmentDate, r.appointmentTime, r.registrationDate,
        b.name AS name, b.lastName AS lastName
    FROM 
        randevu r
    INNER JOIN 
        berber b ON r.barberID = b.barberID
    WHERE 
        r.customerID = ?
");
$stmt->bind_param("i", $customerID);
$stmt->execute();

$result = $stmt->get_result();

$appointments = [];
while ($row = $result->fetch_assoc()) {
    $appointments[] = [
        "name" => $row["name"],
        "lastName" => $row["lastName"],
        "appointmentDate" => $row["appointmentDate"],
        "appointmentTime" => $row["appointmentTime"],
        "registrationDate" => $row["registrationDate"]
    ];
}

echo json_encode(["appointments" => $appointments], JSON_UNESCAPED_UNICODE);

$conn->close();
?>
