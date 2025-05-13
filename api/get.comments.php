<?php

$conn = new mysqli("localhost", "root", "", "berberrandevusistemi");                         // Veritabani baglantisi.
$conn->set_charset("utf8");

if ($conn->connect_error) {                                                                 // Baglanti hatasi kontrolu.
    die("Bağlantı hatası: " . $conn->connect_error);
}

if (!isset($_GET["barberID"])) {                                                       
    echo json_encode(["error" => "barberID parametresi eksik"]);
    exit;
}

$barberID = intval($_GET["barberID"]);

$stmt = $conn->prepare("
    SELECT 
        m.name, 
        m.lastName, 
        y.commentText, 
        y.commentDate, 
        y.barberPoint 
    FROM 
        yorum y
    INNER JOIN 
        musteri m ON y.customerID = m.customerID
    WHERE 
        y.barberID = ?
    ORDER BY 
        y.commentDate DESC
");

$stmt->bind_param("i", $barberID);
$stmt->execute();

$result = $stmt->get_result();
    
$comments = [];
while ($row = $result->fetch_assoc()) {
    $comments[] = [
        'name' => $row['name'],
        'lastName' => $row['lastName'],
        'commentText' => $row['commentText'],
        'commentDate' => date("d-m-Y", strtotime($row['commentDate'])),
        'barberPoint' => (int)$row['barberPoint'],
    ];
}

echo json_encode(["comments" => $comments], JSON_UNESCAPED_UNICODE);

$stmt->close();
$conn->close();

?>
