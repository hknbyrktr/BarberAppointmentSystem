<?php

$conn = new mysqli("localhost", "root", "", "berberrandevusistemi");                                 // Veritabani baglantisi.
$conn->set_charset("utf8");

if ($conn->connect_error) {                                                                          // Hata kontrolu.
    die("Bağlantı hatası: " . $conn->connect_error);
}

$name = $_POST["name"];
$lastName = $_POST["lastName"];
$userName = $_POST["userName"];
$password = $_POST["password"];
$phoneNum = $_POST["phoneNum"];

//$hashedPassword = password_hash($password, PASSWORD_DEFAULT);                                       // Guvenlik icin sifreyi hashle.
$hashedPassword = $password;                                                              // Tabikide mantikli bir hareket degil unity de sifre gozuksun diye bole yaptik. Normalde ust satiri kullan.


$check = $conn->prepare("SELECT * FROM musteri WHERE userName = ?");                                // Ayni kullanici var mi kontrolu.
$check->bind_param("s", $userName);
$check->execute();
$result = $check->get_result();

if ($result->num_rows > 0) {                                                                        // Baska bir kullanici varsa hata dondur.
    echo "exists";
} else {
    $stmt = $conn->prepare("INSERT INTO musteri (name, lastName, userName, phoneNum, password ) VALUES (?, ?, ?, ?, ?)");
    $stmt->bind_param("sssss", $name, $lastName, $userName, $phoneNum, $hashedPassword );
    $stmt->execute();
    echo "success";
    
    $stmt->close();
}

$conn->close();

?>
