-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 06, 2026 at 02:48 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `rmb_coaching`
--
CREATE DATABASE IF NOT EXISTS `rmb_coaching` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `rmb_coaching`;

-- --------------------------------------------------------

--
-- Table structure for table `cartitems`
--

CREATE TABLE `cartitems` (
  `Id` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `CourseId` int(11) NOT NULL,
  `AddedAt` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Table structure for table `courses`
--

CREATE TABLE `courses` (
  `Id` int(11) NOT NULL,
  `Title` varchar(200) NOT NULL,
  `ShortDescription` varchar(300) NOT NULL,
  `Description` text NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `DurationInDays` int(11) NOT NULL,
  `Category` varchar(100) NOT NULL,
  `DifficultyLevel` varchar(50) NOT NULL,
  `ImageUrl` varchar(500) DEFAULT NULL,
  `IsSubscription` tinyint(1) NOT NULL DEFAULT 0,
  `IsActive` tinyint(1) NOT NULL DEFAULT 1,
  `CreatedAt` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Dumping data for table `courses`
--

INSERT INTO `courses` (`Id`, `Title`, `ShortDescription`, `Description`, `Price`, `DurationInDays`, `Category`, `DifficultyLevel`, `ImageUrl`, `IsSubscription`, `IsActive`, `CreatedAt`) VALUES
(1, 'Kezdő tömegnövelő edzésterv', 'Izomtömeg-növelő program kezdőknek', '4 hetes részletes edzésterv és alap étrendi ajánlások.', 7990.00, 30, 'Konditerem', 'Kezdő', '/images/gym.jpg', 0, 1, '2026-04-03 15:46:32'),
(6, 'Haladó mell-hát erősítő program', 'Felsőtest fókuszú tömegnövelő terv', '6 hetes program, amely a mell és hát izomcsoportok fejlesztésére koncentrál. Intenzív edzésnapokkal és progresszív túlterheléssel segíti az erő és izomtömeg növelését.', 10990.00, 42, 'Konditerem', 'Haladó', '/images/gym2.jpg', 0, 1, '2026-04-10 22:04:59'),
(7, 'Alsótest robbanékonyság fejlesztő terv', 'Láb- és farizom fejlesztő edzésterv', '5 hetes program, amely guggolásra, kitörésekre és combhajlító gyakorlatokra épül. Célja a láberő, a robbanékonyság és az izomtömeg fokozása.', 9490.00, 35, 'Konditerem', 'Középhaladó', '/images/gym3.jpg', 0, 1, '2026-04-10 22:07:19'),
(8, 'Szálkásító konditermi program', 'Zsírégetés és izommegtartás egyben', '8 hetes szálkásító program, amely konditermi köredzéseket és intenzív kardiót is tartalmaz. Azoknak ajánlott, akik formásabb, szárazabb fizikumot szeretnének.', 11990.00, 56, 'Konditerem', 'Középhaladó', '/images/gym4.jpg', 1, 1, '2026-04-10 22:08:14'),
(9, 'Erőemelő alapozó program', 'Guggolás, fekvenyomás és felhúzás fókusz', '6 hetes alap erőemelő program, amely a három fő összetett gyakorlat technikáját és erejét fejleszti. Ajánlott azoknak, akik erőben szeretnének nagyot lépni előre.', 12990.00, 42, 'Konditerem', 'Haladó', '/images/gym5.jpg', 0, 1, '2026-04-10 22:08:54'),
(10, 'Kezdő football állóképesség program', 'Futballhoz szükséges alap állóképesség', '4 hetes program, amely a futballban fontos aerob állóképességet és alap futómunkát fejleszti. Kezdő játékosoknak kiváló alapozó terv.', 8490.00, 30, 'Football', 'Kezdő', '/images/fogyas.jpg', 0, 1, '2026-04-10 22:11:26'),
(11, 'Labdakezelési technika fejlesztő edzésterv', 'Pontosabb labdavezetés és kontroll', '5 hetes technikai program bójás gyakorlatokkal, első érintés fejlesztéssel és irányváltásokkal. Célja a magabiztosabb labdakezelés mérkőzéshelyzetben.', 8990.00, 35, 'Football', 'Középhaladó', '/images/fogyas2.jpg', 0, 1, '2026-04-10 22:12:08'),
(12, 'Gyorsaság és sprint football program', 'Gyorsabb indulás és végsebesség', '6 hetes program, amely a sprinttechnikát, rajtgyorsaságot és rövidtávú robbanékonyságot fejleszti. Szélsőknek, csatároknak és gyors játékosoknak különösen hasznos.', 10990.00, 42, 'Football', 'Haladó', '/images/fogyas3.jpg', 0, 1, '2026-04-10 22:12:50'),
(13, 'Mérkőzésre felkészítő football edzés', 'Komplex heti football felkészülés', '4 hetes rendszer, amely technikai, taktikai és erőnléti elemeket is tartalmaz. Célja a meccsterhelés jobb bírhatósága és a kiegyensúlyozott teljesítmény.', 9990.00, 30, 'Football', 'Középhaladó', '/images/fogyas4.jpg', 1, 1, '2026-04-10 22:13:33'),
(14, 'Haladó football robbanékonysági terv', 'Intenzív gyorsulás és irányváltás fejlesztés', '6 hetes haladó edzésterv, amely plyometrikus és gyorsasági gyakorlatokra épül. Olyan játékosoknak ajánlott, akik a pályán dinamikusabbak szeretnének lenni.', 11490.00, 42, 'Football', 'Haladó', '/images/fogyas5.jpg', 0, 1, '2026-04-10 22:14:19'),
(15, 'Mixed teljes testes alapozó program', 'Kondi és állóképesség kombinálva', '4 hetes teljes testes program, amely erősítő és kardiós blokkokat is tartalmaz. Azoknak szól, akik általános formába szeretnének kerülni.', 8990.00, 30, 'Mixed', 'Kezdő', '/images/ero.jpg', 0, 1, '2026-04-10 22:17:02'),
(16, 'Funkcionális erő és mobilitás program', 'Stabilitás, erő és mozgékonyság együtt', '5 hetes mixed program saját testsúlyos, súlyzós és mobilitási gyakorlatokkal. Célja a sérülésmegelőzés és a mindennapi teljesítmény javítása.', 9490.00, 35, 'Mixed', 'Középhaladó', '/images/ero2.jpg', 0, 1, '2026-04-10 22:17:42'),
(17, 'Zsírégető mixed challenge', 'Intenzív formába hozó kihívás', '6 hetes, intenzív mixed program HIIT, erősítő és core gyakorlatokkal. Azoknak ajánlott, akik rövid idő alatt szeretnének látványos változást elérni.', 10490.00, 42, 'Mixed', 'Középhaladó', '/images/ero3.jpg', 1, 1, '2026-04-10 22:18:23'),
(18, 'Haladó mixed athletic program', 'Erő, gyorsaság és állóképesség együtt', '8 hetes haladó edzésterv sportos életmódot követőknek. Kombinálja a súlyzós erőfejlesztést, a futást és a funkcionális mozgásokat.', 12490.00, 56, 'Mixed', 'Haladó', '/images/ero4.jpg', 0, 1, '2026-04-10 22:18:59'),
(19, 'Otthoni és konditermi mixed átalakuló program', 'Rugalmas edzésterv minden környezetre', '7 hetes program, amely otthoni és konditermi napokat is tartalmaz. Azoknak tökéletes, akik változatosan szeretnének fejlődni és nem mindig ugyanott edzenek.', 10990.00, 49, 'Mixed', 'Középhaladó', '/images/ero5.jpg', 1, 1, '2026-04-10 22:19:42');

-- --------------------------------------------------------

--
-- Table structure for table `orders`
--

CREATE TABLE `orders` (
  `Id` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `CourseId` int(11) NOT NULL,
  `PriceAtPurchase` decimal(10,2) NOT NULL,
  `OrderDate` datetime NOT NULL DEFAULT current_timestamp(),
  `IsCancelled` tinyint(1) NOT NULL DEFAULT 0,
  `CancelledAt` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `Id` int(11) NOT NULL,
  `FullName` varchar(150) NOT NULL,
  `Email` varchar(150) NOT NULL,
  `PhoneNumber` varchar(30) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Role` varchar(20) NOT NULL DEFAULT 'User',
  `CreatedAt` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`Id`, `FullName`, `Email`, `PhoneNumber`, `PasswordHash`, `Role`, `CreatedAt`) VALUES
(1, 'Admin', 'admin@rmb.hu', '0101010101', '100000.qR70+UoFRNMltYvcx0EVXg==.4ur/KP3Hs9ngtBLs53BjAi5Sqch452xpTPRrOMB5vJA=', 'Admin', '2026-04-14 20:12:49'),
(2, 'balazs', 'balazs@balazs', 'balazs', '100000.5X4VpzM8Sh0P1I1K4sCsOQ==.vPHS0JeOj5oCGt0R1+gOdLhzQMKIbS5Wl5fjdDPC5Hs=', 'User', '2026-04-06 14:33:40'),
(3, 'botond', 'botond@botond.com', '06111111111', '100000.WjnVItBWm9SC+b+HSvw0QQ==.zaN+r3lkyzBs5sj99v6tXgpYVJQH+MjTtWMKg49dfDw=', 'Admin', '2026-04-08 18:56:50'),
(4, 'balazs', 'balazs@balazs.balazs', 'balazs', '100000.dEFLVuJFRa6T67N7i3bQjg==.7EXJ4cYdmu3GhG7yK9edBcLWhiwPb8OVAh+2oLZZbww=', 'Admin', '2026-04-06 14:09:31'),
(5, 'teszt', 'teszt@teszt.com', 'teszt', '100000.7GD9GSzN5UHjrR1JoN/4GQ==.uSLWmJtPMOSwsRipd+wlMfzegh+0uYxwtmfixQ5XA9Q=', 'User', '2026-04-10 10:59:33'),
(7, 'bot', 'bot@bot', 'bot', '100000.DQa7hoGgVlBSW8fXVUighg==.u7hMgqeybe5LhNwuDU9y+H+robjEdK3SKbPisfpsjiM=', 'Admin', '2026-04-10 21:46:26');
--
-- Indexes for dumped tables
--

--
-- Indexes for table `cartitems`
--
ALTER TABLE `cartitems`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `UQ_CartItems_UserId` (`UserId`),
  ADD KEY `FK_CartItems_Courses` (`CourseId`);

--
-- Indexes for table `courses`
--
ALTER TABLE `courses`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FK_Orders_Users` (`UserId`),
  ADD KEY `FK_Orders_Courses` (`CourseId`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `UQ_Users_Email` (`Email`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `cartitems`
--
ALTER TABLE `cartitems`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `courses`
--
ALTER TABLE `courses`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `orders`
--
ALTER TABLE `orders`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `cartitems`
--
ALTER TABLE `cartitems`
  ADD CONSTRAINT `FK_CartItems_Courses` FOREIGN KEY (`CourseId`) REFERENCES `courses` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_CartItems_Users` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `FK_Orders_Courses` FOREIGN KEY (`CourseId`) REFERENCES `courses` (`Id`),
  ADD CONSTRAINT `FK_Orders_Users` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
