<?php

$conn = new mysqli("localhost", "root", "", "berberrandevusistemi");                         // Veritabani baglantisi.
$conn->set_charset("utf8");

if ($conn->connect_error) {
    die("Bağlantı hatası: " . $conn->connect_error);
}

$result = $conn->query("SELECT barberID, name, lastName FROM berber");                      // Butun berberleri getir.

$barbers = [];

while ($row = $result->fetch_assoc()) {
    $barbers[] = [
        'barberID' => (int)$row['barberID'],
        'name' => $row['name'],
        'lastName' => $row['lastName']
    ];
}

echo json_encode(["barbers" => $barbers], JSON_UNESCAPED_UNICODE);

$conn->close();
?>
