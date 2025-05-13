<?php

$conn = new mysqli("localhost", "root", "", "berberrandevusistemi");                         // Veritabani baglantisi
$conn->set_charset("utf8");

if ($conn->connect_error) {                                                                  // Hata kontrolu.
    die("Bağlantı hatası: " . $conn->connect_error);
}

$barberID = $_GET['barberID'];

$stmt = $conn->prepare("SELECT DISTINCT appointmentDate 
FROM randevu 
WHERE barberID = ?
  AND customerID IS NULL
  AND registrationDate IS NULL
  ORDER BY appointmentDate ASC
");                                                                                          // Randevu tarihlerini getir. Ama onceden alinmamis olanlari.

$stmt->bind_param("i", $barberID);

$stmt->execute();
$result = $stmt->get_result();

$dates = [];
while ($row = $result->fetch_assoc()) {
    $dates[] = ['appointmentDate' => $row['appointmentDate']];
}

echo json_encode(["dates" => $dates], JSON_UNESCAPED_UNICODE);



$stmt->close();
$conn->close();

?>
