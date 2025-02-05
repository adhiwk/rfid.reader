/*
 Navicat Premium Data Transfer

 Source Server         : localhost_mariadb
 Source Server Type    : MariaDB
 Source Server Version : 110602 (11.6.2-MariaDB)
 Source Host           : localhost:3306
 Source Schema         : rfid_reader

 Target Server Type    : MariaDB
 Target Server Version : 110602 (11.6.2-MariaDB)
 File Encoding         : 65001

 Date: 02/02/2025 21:00:05
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for peserta
-- ----------------------------
DROP TABLE IF EXISTS `peserta`;
CREATE TABLE `peserta`  (
  `nomor_peserta` varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `nama_peserta` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `bib` varchar(5) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `tag` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 101 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of peserta
-- ----------------------------
INSERT INTO `peserta` VALUES ('P001', 'Andi Setiawan', 'B001', '8463957281', 'Belum', 1);
INSERT INTO `peserta` VALUES ('P002', 'Budi Santoso', 'B002', '9315728460', 'Belum', 2);
INSERT INTO `peserta` VALUES ('P003', 'Citra Dewi', 'B003', '5128746930', 'Belum', 3);
INSERT INTO `peserta` VALUES ('P004', 'Diana Sari', 'B004', '3485162907', 'Belum', 4);
INSERT INTO `peserta` VALUES ('P005', 'Eka Prasetya', 'B005', '2904681537', 'Belum', 5);
INSERT INTO `peserta` VALUES ('P006', 'Fajar Hidayat', 'B006', '7634592185', 'Belum', 6);
INSERT INTO `peserta` VALUES ('P007', 'Gina Lestari', 'B007', '9452078136', 'Belum', 7);
INSERT INTO `peserta` VALUES ('P008', 'Hendra Yudha', 'B008', '8624739051', 'Belum', 8);
INSERT INTO `peserta` VALUES ('P009', 'Indah Puspita', 'B009', '6579024813', 'Belum', 9);
INSERT INTO `peserta` VALUES ('P010', 'Joko Sutrisno', 'B010', '7134862597', 'Belum', 10);
INSERT INTO `peserta` VALUES ('P011', 'Kurniawan Ahmad', 'B011', '5326089471', 'Belum', 11);
INSERT INTO `peserta` VALUES ('P012', 'Lina Melati', 'B012', '8125397640', 'Belum', 12);
INSERT INTO `peserta` VALUES ('P013', 'Maya Putri', 'B013', '9304875612', 'Belum', 13);
INSERT INTO `peserta` VALUES ('P014', 'Nina Pratama', 'B014', '1284975063', 'Belum', 14);
INSERT INTO `peserta` VALUES ('P015', 'Oscar Ardianto', 'B015', '5643702918', 'Belum', 15);
INSERT INTO `peserta` VALUES ('P016', 'Putu Wijaya', 'B016', '7826514930', 'Belum', 16);
INSERT INTO `peserta` VALUES ('P017', 'Qia Nadira', 'B017', '9408362157', 'Belum', 17);
INSERT INTO `peserta` VALUES ('P018', 'Rina Amalia', 'B018', '6247851930', 'Belum', 18);
INSERT INTO `peserta` VALUES ('P019', 'Satria Dewantara', 'B019', '3485612794', 'Belum', 19);
INSERT INTO `peserta` VALUES ('P020', 'Tina Ratna', 'B020', '1956304782', 'Belum', 20);
INSERT INTO `peserta` VALUES ('P021', 'Umar Fadil', 'B021', '8462039751', 'Belum', 21);
INSERT INTO `peserta` VALUES ('P022', 'Vera Wulandari', 'B022', '5639071248', 'Belum', 22);
INSERT INTO `peserta` VALUES ('P023', 'Widi Rahmat', 'B023', '7843652901', 'Belum', 23);
INSERT INTO `peserta` VALUES ('P024', 'Xenia Prawira', 'B024', '9275614308', 'Belum', 24);
INSERT INTO `peserta` VALUES ('P025', 'Yani Supriyadi', 'B025', '1867435209', 'Belum', 25);
INSERT INTO `peserta` VALUES ('P026', 'Zahra Eliza', 'B026', '4973061825', 'Belum', 26);
INSERT INTO `peserta` VALUES ('P027', 'Agus Santosa', 'B027', '3219685740', 'Belum', 27);
INSERT INTO `peserta` VALUES ('P028', 'Beny Yulianto', 'B028', '6934718205', 'Belum', 28);
INSERT INTO `peserta` VALUES ('P029', 'Clara Batubara', 'B029', '2057468391', 'Belum', 29);
INSERT INTO `peserta` VALUES ('P030', 'Darto Prakoso', 'B030', '3728409516', 'Belum', 30);
INSERT INTO `peserta` VALUES ('P031', 'Elsa Cahyani', 'B031', '9052746318', 'Belum', 31);
INSERT INTO `peserta` VALUES ('P032', 'Farhan Malik', 'B032', '6432571980', 'Belum', 32);
INSERT INTO `peserta` VALUES ('P033', 'Gilang Cahyono', 'B033', '7216985304', 'Belum', 33);
INSERT INTO `peserta` VALUES ('P034', 'Haryanto Nur', 'B034', '4987613520', 'Belum', 34);
INSERT INTO `peserta` VALUES ('P035', 'Isabella Putri', 'B035', '2105694873', 'Belum', 35);
INSERT INTO `peserta` VALUES ('P036', 'Jamilah Azizah', 'B036', '6712954832', 'Belum', 36);
INSERT INTO `peserta` VALUES ('P037', 'Karla Wati', 'B037', '8451073926', 'Belum', 37);
INSERT INTO `peserta` VALUES ('P038', 'Luthfi Harun', 'B038', '5829307461', 'Belum', 38);
INSERT INTO `peserta` VALUES ('P039', 'Mia Adisty', 'B039', '2746315098', 'Belum', 39);
INSERT INTO `peserta` VALUES ('P040', 'Nino Adrian', 'B040', '5397620841', 'Belum', 40);
INSERT INTO `peserta` VALUES ('P041', 'Oka Susanto', 'B041', '6173589240', 'Belum', 41);
INSERT INTO `peserta` VALUES ('P042', 'Putri Sekar', 'B042', '2958143670', 'Belum', 42);
INSERT INTO `peserta` VALUES ('P043', 'Qaisara Alamsyah', 'B043', '3084692157', 'Belum', 43);
INSERT INTO `peserta` VALUES ('P044', 'Rina Hermawati', 'B044', '4632958701', 'Belum', 44);
INSERT INTO `peserta` VALUES ('P045', 'Siti Aisyah', 'B045', '5609342718', 'Belum', 45);
INSERT INTO `peserta` VALUES ('P046', 'Taufik Hidayat', 'B046', '8231504796', 'Belum', 46);
INSERT INTO `peserta` VALUES ('P047', 'Umar Syarif', 'B047', '3648710592', 'Belum', 47);
INSERT INTO `peserta` VALUES ('P048', 'Vina Rahayu', 'B048', '7245803196', 'Belum', 48);
INSERT INTO `peserta` VALUES ('P049', 'Wahyu Nugroho', 'B049', '5139460287', 'Belum', 49);
INSERT INTO `peserta` VALUES ('P050', 'Xander Putra', 'B050', '6027384591', 'Belum', 50);
INSERT INTO `peserta` VALUES ('P051', 'Yulianto Setiawan', 'B051', '4175639280', 'Belum', 51);
INSERT INTO `peserta` VALUES ('P052', 'Zaneta Putri', 'B052', '7596208134', 'Belum', 52);
INSERT INTO `peserta` VALUES ('P053', 'Agus Pratama', 'B053', '2486539710', 'Belum', 53);
INSERT INTO `peserta` VALUES ('P054', 'Beni Fauzan', 'B054', '7812064935', 'Belum', 54);
INSERT INTO `peserta` VALUES ('P055', 'Cynthia Lestari', 'B055', '1627403859', 'Belum', 55);
INSERT INTO `peserta` VALUES ('P056', 'Dian Wijaya', 'B056', '5073819642', 'Belum', 56);
INSERT INTO `peserta` VALUES ('P057', 'Eko Suryanto', 'B057', '3285196740', 'Belum', 57);
INSERT INTO `peserta` VALUES ('P058', 'Farida Lestari', 'B058', '4926758130', 'Belum', 58);
INSERT INTO `peserta` VALUES ('P059', 'Gani Fikri', 'B059', '2359467108', 'Belum', 59);
INSERT INTO `peserta` VALUES ('P060', 'Hana Kartika', 'B060', '6732581490', 'Belum', 60);
INSERT INTO `peserta` VALUES ('P061', 'Irwan Suprayitno', 'B061', '5134286907', 'Belum', 61);
INSERT INTO `peserta` VALUES ('P062', 'Jessi Wulansari', 'B062', '8142569302', 'Belum', 62);
INSERT INTO `peserta` VALUES ('P063', 'Kevin Triananda', 'B063', '9086315472', 'Belum', 63);
INSERT INTO `peserta` VALUES ('P064', 'Larasati Nur', 'B064', '6134892750', 'Belum', 64);
INSERT INTO `peserta` VALUES ('P065', 'Mira Elva', 'B065', '5407291863', 'Belum', 65);
INSERT INTO `peserta` VALUES ('P066', 'Nina Sukmawati', 'B066', '8527306491', 'Belum', 66);
INSERT INTO `peserta` VALUES ('P067', 'Oki Cahyadi', 'B067', '4927318560', 'Belum', 67);
INSERT INTO `peserta` VALUES ('P068', 'Prasetya Aji', 'B068', '3586079142', 'Belum', 68);
INSERT INTO `peserta` VALUES ('P069', 'Qina Aulia', 'B069', '1709356428', 'Belum', 69);
INSERT INTO `peserta` VALUES ('P070', 'Rian Sanjaya', 'B070', '2456809731', 'Belum', 70);
INSERT INTO `peserta` VALUES ('P071', 'Satria Hadi', 'B071', '4963205871', 'Belum', 71);
INSERT INTO `peserta` VALUES ('P072', 'Toni Indrayani', 'B072', '6197583042', 'Belum', 72);
INSERT INTO `peserta` VALUES ('P073', 'Umi Maulidah', 'B073', '3287165049', 'Belum', 73);
INSERT INTO `peserta` VALUES ('P074', 'Vicky Prabowo', 'B074', '2305986471', 'Belum', 74);
INSERT INTO `peserta` VALUES ('P075', 'Wendi Taufiq', 'B075', '7625403917', 'Belum', 75);
INSERT INTO `peserta` VALUES ('P076', 'Xena Damayanti', 'B076', '8457692031', 'Belum', 76);
INSERT INTO `peserta` VALUES ('P077', 'Yosua Pratama', 'B077', '5702918463', 'Belum', 77);
INSERT INTO `peserta` VALUES ('P078', 'Zulfiqar Kamal', 'B078', '3912746508', 'Belum', 78);
INSERT INTO `peserta` VALUES ('P079', 'Amira Gita', 'B079', '8364712509', 'Belum', 79);
INSERT INTO `peserta` VALUES ('P080', 'Bintang Laksana', 'B080', '6195082734', 'Belum', 80);
INSERT INTO `peserta` VALUES ('P081', 'Cahyo Agung', 'B081', '7953620481', 'Belum', 81);
INSERT INTO `peserta` VALUES ('P082', 'Devi Sari', 'B082', '3945812760', 'Belum', 82);
INSERT INTO `peserta` VALUES ('P083', 'Endang Rosita', 'B083', '6029475813', 'Belum', 83);
INSERT INTO `peserta` VALUES ('P084', 'Ferry Andika', 'B084', '7105386429', 'Belum', 84);
INSERT INTO `peserta` VALUES ('P085', 'Gordon Surya', 'B085', '4162039758', 'Belum', 85);
INSERT INTO `peserta` VALUES ('P086', 'Harris Ramadhan', 'B086', '1097638452', 'Belum', 86);
INSERT INTO `peserta` VALUES ('P087', 'Ilham Akbar', 'B087', '4076518923', 'Belum', 87);
INSERT INTO `peserta` VALUES ('P088', 'Jamilah Karina', 'B088', '5089367215', 'Belum', 88);
INSERT INTO `peserta` VALUES ('P089', 'Krisna Yoga', 'B089', '2905147368', 'Belum', 89);
INSERT INTO `peserta` VALUES ('P090', 'Luthfi Kurniawan', 'B090', '3612409756', 'Belum', 90);
INSERT INTO `peserta` VALUES ('P091', 'Maya Rudi', 'B091', '4708529361', 'Belum', 91);
INSERT INTO `peserta` VALUES ('P092', 'Nia Damayanti', 'B092', '2974185360', 'Belum', 92);
INSERT INTO `peserta` VALUES ('P093', 'Omar Hidayat', 'B093', '1837492560', 'Belum', 93);
INSERT INTO `peserta` VALUES ('P094', 'Putri Maharani', 'B094', '9526714308', 'Belum', 94);
INSERT INTO `peserta` VALUES ('P095', 'Rizki Amrullah', 'B095', '5142903876', 'Belum', 95);
INSERT INTO `peserta` VALUES ('P096', 'Siti Hartini', 'B096', '3425097186', 'Belum', 96);
INSERT INTO `peserta` VALUES ('P097', 'Titi Rahayu', 'B097', '6403728519', 'Belum', 97);
INSERT INTO `peserta` VALUES ('P098', 'Umar Syahri', 'B098', '7594312058', 'Belum', 98);
INSERT INTO `peserta` VALUES ('P099', 'Vira Kurnia', 'B099', '8164273950', 'Belum', 99);
INSERT INTO `peserta` VALUES ('P100', 'Widyawati Nugroho', 'B100', '5427061938', 'Belum', 100);

SET FOREIGN_KEY_CHECKS = 1;
