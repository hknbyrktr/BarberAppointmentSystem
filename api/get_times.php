<?php

$conn = new mysqli("localhost", "root", "", "berberrandevusistemi");                                // Veritabani baglantisi.

$conn->set_charset("utf8");                                                                         // Karakter setini ayarla.


if ($conn->connect_error) {                                                                         // Hata kontrolu.
    die("Bağlantı hatası: " . $conn->connect_error);
}

$appointmentDate = $_POST['appointmentDate'];                                                       // Unity'den gelen degerleri al.
$barberID = $_POST['barberID'];

                                                                                                    
$stmt = $conn->prepare("SELECT appointmentTime 
FROM randevu 
WHERE appointmentDate = ?
  AND barberID = ?
  AND customerID IS NULL
  AND registrationDate IS NULL 
  ORDER BY appointmentTime ASC
");                                                                                                 // Randevu zamanlarini getir. Ama onceden alinmamis olanlari.

$stmt->bind_param("si", $appointmentDate, $barberID);
$stmt->execute();
$result = $stmt->get_result();


$times = [];
while ($row = $result->fetch_assoc()) {
    $times[] = ['appointmentTime' => $row['appointmentTime']];
}

echo json_encode(["times" => $times], JSON_UNESCAPED_UNICODE);


$stmt->close();                                                                                     // En son herseyi kapat.
$conn->close();
?>
