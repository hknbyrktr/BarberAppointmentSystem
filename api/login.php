<?php

$conn = new mysqli("localhost", "root", "", "berberrandevusistemi");                                // Veritabani baglantisi
$conn->set_charset("utf8");

if ($conn->connect_error) {                                                                         // Hata kontrolu.
    die("Bağlantı hatası: " . $conn->connect_error);
}

$userName = $_POST["userName"];
$password = $_POST["password"];

$stmt = $conn->prepare("SELECT customerID, password FROM musteri WHERE userName = ?");
$stmt->bind_param("s", $userName);
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {                                                                    
    $row = $result->fetch_assoc();
    //if (password_verify($password, $row["password"])) {                                          // Hashing kullanmadigimiz icin buraya gerek yok.
    if($password == $row["password"])
    {
        echo "success";
    } 
    else 
    {
        echo "wrong";
    }
}
else 
{
   echo "notExist";
}
$stmt->close();
$conn->close();

?>
