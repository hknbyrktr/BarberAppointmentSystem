-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Anamakine: 127.0.0.1
-- Üretim Zamanı: 13 May 2025, 18:29:12
-- Sunucu sürümü: 10.4.32-MariaDB
-- PHP Sürümü: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Veritabanı: `berberrandevusistemi`
--

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `berber`
--

CREATE TABLE `berber` (
  `barberID` int(11) NOT NULL,
  `name` varchar(50) DEFAULT NULL,
  `lastName` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Tablo döküm verisi `berber`
--

INSERT INTO `berber` (`barberID`, `name`, `lastName`) VALUES
(1, 'Osman', 'Gültekin'),
(2, 'Şinasi', 'Yılmaz'),
(3, 'Dursun', 'Durmaz'),
(4, 'Şemset', 'Sabret'),
(5, 'Hakkı', 'Hakyemez'),
(6, 'Alex', 'de Souza'),
(7, 'Dulkadir', 'Baltacı'),
(8, 'Sabri', 'Sarıoğlu'),
(9, 'Brad', 'Pitt');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `hizmet`
--

CREATE TABLE `hizmet` (
  `serviceID` int(11) NOT NULL,
  `serviceName` varchar(100) DEFAULT NULL,
  `price` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Tablo döküm verisi `hizmet`
--

INSERT INTO `hizmet` (`serviceID`, `serviceName`, `price`) VALUES
(1, 'Saç Tıraşı', 100.00),
(2, 'Sakal Tıraşı', 50.00),
(3, 'Saç Yıkama', 120.00),
(4, 'Saç Boyama', 150.00);

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `musteri`
--

CREATE TABLE `musteri` (
  `customerID` int(11) NOT NULL,
  `name` varchar(50) DEFAULT NULL,
  `lastName` varchar(50) DEFAULT NULL,
  `phoneNum` varchar(20) DEFAULT NULL,
  `userName` varchar(50) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Tablo döküm verisi `musteri`
--

INSERT INTO `musteri` (`customerID`, `name`, `lastName`, `phoneNum`, `userName`, `password`) VALUES
(17, 'Muzaffer', 'Topçu', '5466465565', 'top_muz', '$2y$10$nmfde63YO3EUAxvyFC68G.fF6bfJdW7hUQtlNXqwYsRuqH8lzWAz2'),
(18, 'Hakkı', 'Hacıosmanoğlu', '5349295299', 'Hacıoğlu', '$2y$10$vG5PukTfsyStGtXIylsls.kcCbc0I9TpAUeN611TOzRGb9/K4.WAa'),
(19, 'Hamzet', 'Zahmet', '5555555555', 'hamzah', 'hamzah'),
(20, 'Ahmet', 'Düşmez', '5555555555', 'ahmet54', 'ahmet54'),
(21, NULL, NULL, NULL, NULL, NULL),
(22, 'Osman ', 'Gültekin', '5463333333', 'osgül', 'osgül'),
(23, NULL, NULL, NULL, NULL, NULL),
(24, NULL, NULL, NULL, NULL, NULL),
(25, 'Batu', 'Hak', '5555555555', 'hak', 'hak'),
(26, NULL, NULL, NULL, NULL, NULL),
(27, NULL, NULL, NULL, NULL, NULL),
(28, NULL, NULL, NULL, NULL, NULL),
(29, NULL, NULL, NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `randevu`
--

CREATE TABLE `randevu` (
  `appointmentID` int(11) NOT NULL,
  `barberID` int(11) DEFAULT NULL,
  `customerID` int(11) DEFAULT NULL,
  `appointmentDate` date DEFAULT NULL,
  `appointmentTime` time DEFAULT NULL,
  `registrationDate` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Tablo döküm verisi `randevu`
--

INSERT INTO `randevu` (`appointmentID`, `barberID`, `customerID`, `appointmentDate`, `appointmentTime`, `registrationDate`) VALUES
(3, 3, NULL, '2025-05-15', '11:00:00', NULL),
(4, 4, NULL, '2025-05-15', '12:00:00', NULL),
(5, 5, NULL, '2025-05-16', '14:00:00', NULL),
(6, 1, 19, '2025-05-15', '15:00:00', '2025-05-12 22:38:00'),
(7, 2, NULL, '2025-05-16', '16:00:00', NULL),
(8, 3, NULL, '2025-05-17', '17:00:00', NULL),
(9, 4, NULL, '2025-05-18', '18:00:00', NULL),
(10, 5, NULL, '2025-05-16', '19:00:00', NULL),
(12, 5, NULL, '2025-05-15', '15:00:00', NULL),
(13, 5, NULL, '2025-05-16', '15:00:00', NULL),
(14, 5, NULL, '2025-05-15', '12:00:00', NULL),
(15, 5, NULL, '2025-05-17', '12:00:00', NULL),
(16, 1, NULL, '2025-05-19', '12:00:00', NULL),
(17, 1, NULL, '2025-05-16', '12:00:00', NULL),
(18, 1, NULL, '2025-05-16', '14:00:00', NULL),
(19, 1, NULL, '2025-05-17', '14:00:00', NULL),
(20, 2, NULL, '2025-05-17', '14:00:00', NULL),
(21, 2, NULL, '2025-05-16', '14:00:00', NULL),
(22, 2, NULL, '2025-05-18', '14:00:00', NULL),
(23, 2, NULL, '2025-05-19', '14:00:00', NULL),
(24, 2, NULL, '2025-05-18', '15:00:00', NULL),
(25, 3, NULL, '2025-05-18', '15:00:00', NULL),
(26, 3, NULL, '2025-05-18', '11:00:00', NULL),
(27, 3, NULL, '2025-05-17', '09:00:00', NULL),
(28, 4, NULL, '2025-05-17', '09:00:00', NULL),
(29, 4, NULL, '2025-05-17', '10:00:00', NULL),
(30, 4, NULL, '2025-05-15', '09:00:00', NULL),
(31, 1, 25, '2025-05-15', '11:00:00', '2025-05-13 17:09:09');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `randevuhizmet`
--

CREATE TABLE `randevuhizmet` (
  `appointmentID` int(11) NOT NULL,
  `serviceID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Tablo döküm verisi `randevuhizmet`
--

INSERT INTO `randevuhizmet` (`appointmentID`, `serviceID`) VALUES
(6, 1),
(6, 3),
(31, 1),
(31, 3),
(31, 4);

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `yorum`
--

CREATE TABLE `yorum` (
  `commentID` int(11) NOT NULL,
  `barberID` int(11) DEFAULT NULL,
  `customerID` int(11) DEFAULT NULL,
  `commentText` text DEFAULT NULL,
  `commentDate` date DEFAULT current_timestamp(),
  `barberPoint` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Tablo döküm verisi `yorum`
--

INSERT INTO `yorum` (`commentID`, `barberID`, `customerID`, `commentText`, `commentDate`, `barberPoint`) VALUES
(33, 1, 17, 'Traşı gayet güzeldi. Sonunda istediğini değil istediğimi yapan bir berber :)\r\n', '2025-05-12', 5),
(34, 2, 17, 'Gayet güzel bir traştı. Ellerine sağlık', '2025-05-12', 5),
(35, 8, 17, 'Futbolu batırdı sıra berberliğe geldi. ', '2025-05-12', 1),
(36, 1, 18, 'Fena değildi.', '2025-05-12', 4),
(37, 5, 18, 'Fena değildi.', '2025-05-12', 3),
(38, 6, 18, 'Gördüğüm en iyi berberdi. 5 yıldız az ama sınır o :)', '2025-05-12', 5),
(39, 1, 19, 'Temiz işçilik', '2025-05-12', 4),
(40, 5, 19, 'Güzel traş etii sağolsun. Ama dükkan temiz değildi bir yıldız ordan kırdım.', '2025-05-12', 4),
(41, 6, 19, 'Her yerde yıldız olan o adam.', '2025-05-12', 5),
(42, 3, 19, 'Ehh işte.', '2025-05-13', 3),
(43, 4, 19, 'Dükkan leş gibiydi insan az bakım yapar.', '2025-05-13', 1),
(44, 1, 20, 'Osman Gültekin | İstanbul Aydın Üniversitesi. İstanbul Aydın Üniversitesi Siyaset Bilimi ve Uluslararası İlişkiler Bölümü Öğretim Üyesi, Uluslararası İlişkiler Direktör Yardımcısı ve UNESCO Kültürel Diplomasi, Yönetişim ve Eğitim Kürsüsü Başkanı Dr.', '2025-05-13', 4),
(45, 4, 20, 'Kendisini uzun yıllardır tanıyorum abim gibi oldu adamın dibi.', '2025-05-13', 5),
(46, 1, 22, 'Gayet iyi.', '2025-05-13', 4);

--
-- Dökümü yapılmış tablolar için indeksler
--

--
-- Tablo için indeksler `berber`
--
ALTER TABLE `berber`
  ADD PRIMARY KEY (`barberID`);

--
-- Tablo için indeksler `hizmet`
--
ALTER TABLE `hizmet`
  ADD PRIMARY KEY (`serviceID`);

--
-- Tablo için indeksler `musteri`
--
ALTER TABLE `musteri`
  ADD PRIMARY KEY (`customerID`);

--
-- Tablo için indeksler `randevu`
--
ALTER TABLE `randevu`
  ADD PRIMARY KEY (`appointmentID`),
  ADD KEY `BerberId` (`barberID`),
  ADD KEY `MusteriId` (`customerID`);

--
-- Tablo için indeksler `randevuhizmet`
--
ALTER TABLE `randevuhizmet`
  ADD PRIMARY KEY (`appointmentID`,`serviceID`),
  ADD KEY `HizmetId` (`serviceID`);

--
-- Tablo için indeksler `yorum`
--
ALTER TABLE `yorum`
  ADD PRIMARY KEY (`commentID`),
  ADD KEY `MusteriId` (`customerID`),
  ADD KEY `barberID` (`barberID`);

--
-- Dökümü yapılmış tablolar için AUTO_INCREMENT değeri
--

--
-- Tablo için AUTO_INCREMENT değeri `berber`
--
ALTER TABLE `berber`
  MODIFY `barberID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- Tablo için AUTO_INCREMENT değeri `hizmet`
--
ALTER TABLE `hizmet`
  MODIFY `serviceID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Tablo için AUTO_INCREMENT değeri `musteri`
--
ALTER TABLE `musteri`
  MODIFY `customerID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;

--
-- Tablo için AUTO_INCREMENT değeri `randevu`
--
ALTER TABLE `randevu`
  MODIFY `appointmentID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;

--
-- Tablo için AUTO_INCREMENT değeri `yorum`
--
ALTER TABLE `yorum`
  MODIFY `commentID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=47;

--
-- Dökümü yapılmış tablolar için kısıtlamalar
--

--
-- Tablo kısıtlamaları `randevu`
--
ALTER TABLE `randevu`
  ADD CONSTRAINT `randevu_ibfk_1` FOREIGN KEY (`barberID`) REFERENCES `berber` (`barberID`),
  ADD CONSTRAINT `randevu_ibfk_2` FOREIGN KEY (`customerID`) REFERENCES `musteri` (`customerID`);

--
-- Tablo kısıtlamaları `randevuhizmet`
--
ALTER TABLE `randevuhizmet`
  ADD CONSTRAINT `randevuhizmet_ibfk_1` FOREIGN KEY (`appointmentID`) REFERENCES `randevu` (`appointmentID`),
  ADD CONSTRAINT `randevuhizmet_ibfk_2` FOREIGN KEY (`serviceID`) REFERENCES `hizmet` (`serviceID`);

--
-- Tablo kısıtlamaları `yorum`
--
ALTER TABLE `yorum`
  ADD CONSTRAINT `yorum_ibfk_2` FOREIGN KEY (`customerID`) REFERENCES `musteri` (`customerID`),
  ADD CONSTRAINT `yorum_ibfk_3` FOREIGN KEY (`barberID`) REFERENCES `berber` (`barberID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
