<?php

$conn = new mysqli("localhost", "root", "", "berberrandevusistemi");                         // Veritabani baglantisi.
$conn->set_charset("utf8");

$userName     = $_POST["userName"];                                                          // Form verilerini al.
$barberID     = $_POST["barberID"];
$commentText  = $_POST["commentText"];
$commentDate  = $_POST["commentDate"];
$barberPoint  = $_POST["barberPoint"];

$stmt = $conn->prepare("SELECT customerID FROM musteri WHERE userName = ?");                // Kullanici adina gore customerID'yi al.
$stmt->bind_param("s", $userName);
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows === 0) {
    echo "user_not_found";
    exit;
}

$row = $result->fetch_assoc();
$customerID = $row["customerID"];
$stmt->close();

$stmt2 = $conn->prepare("INSERT INTO yorum (customerID, barberID, commentText, commentDate, barberPoint) VALUES (?, ?, ?, ?, ?) ");     // Yorum ekle
$stmt2->bind_param("iissi", $customerID, $barberID, $commentText, $commentDate, $barberPoint);

if ($stmt2->execute()) {
    echo "Success";
} else {
    echo "error: " . $stmt2->error;
}

$stmt2->close();
$conn->close();
?>
