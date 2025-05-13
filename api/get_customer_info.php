<?php

$conn = new mysqli("localhost", "root", "", "berberrandevusistemi");
$conn->set_charset("utf8");

if ($conn->connect_error) {                                                                      
    die("Bağlantı hatası: " . $conn->connect_error);
}

if (!isset($_GET['userName'])) {
    echo "userName parametresi eksik. Lütfen çıkış yapıp tekrar bilgilerinizi doğrulayın";
    exit();
}

$userName = $_GET['userName'];

$stmt = $conn->prepare("SELECT name, lastName, userName, password, phoneNum FROM musteri WHERE userName = ?");
$stmt->bind_param("s", $userName);

$stmt->execute();
$result = $stmt->get_result();


if ($result->num_rows === 0) {
    echo "userNotExist";
    exit;
}

$info = [];
while ($row = $result->fetch_assoc()) {
    $info[] = [
        'name' => $row['name'],
        'lastName' => $row['lastName'],
        'userName' => $row['userName'],
        'password' => $row['password'],
        'phoneNum' => $row['phoneNum']
    ];
}


echo json_encode(["info" => $info], JSON_UNESCAPED_UNICODE);

$stmt->close();
$conn->close();
?>
