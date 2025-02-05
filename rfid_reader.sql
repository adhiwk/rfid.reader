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

 Date: 02/02/2025 19:15:41
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for lap_record
-- ----------------------------
DROP TABLE IF EXISTS `lap_record`;
CREATE TABLE `lap_record`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `putaran` int(11) NULL DEFAULT NULL,
  `time_stamp` timestamp NULL DEFAULT NULL,
  `id_peserta` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of lap_record
-- ----------------------------

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
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of peserta
-- ----------------------------
INSERT INTO `peserta` VALUES ('1234', 'AHMAD IRHAM', '12345', '77889123', 'Belum', 1);
INSERT INTO `peserta` VALUES ('', 'SANJAY DUTH GAGAH', '03839', '98893344', 'Belum', 2);
INSERT INTO `peserta` VALUES ('', 'KUMAR JAYSAN', '92839', '93848472', 'Belum', 3);
INSERT INTO `peserta` VALUES ('1237', 'DINAR KHAN', '38499', '19283748', 'Belum', 4);

-- ----------------------------
-- Procedure structure for add_peserta
-- ----------------------------
DROP PROCEDURE IF EXISTS `add_peserta`;
delimiter ;;
CREATE PROCEDURE `add_peserta`(OUT p_error_message VARCHAR(255),
    OUT p_id INT,
    IN p_nomor_peserta VARCHAR(25),
    IN p_nama_peserta VARCHAR(100),
    IN p_bib VARCHAR(5),
    IN p_tag VARCHAR(10),
    IN p_status VARCHAR(50))
BEGIN
    -- Menangani kesalahan menggunakan deklarasi handler
    DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
        -- Rollback jika ada error
        ROLLBACK;
        SET p_error_message = 'Terjadi kesalahan saat insert data';
        SET p_id = NULL;
    END;

    -- Inisialisasi nilai awal
    SET p_error_message = NULL;
    SET p_id = NULL;

    -- Mulai transaksi
    START TRANSACTION;

    -- Insert data peserta (id otomatis karena AUTO_INCREMENT)
    INSERT INTO peserta (nomor_peserta, nama_peserta, bib, tag, status)
    VALUES (p_nomor_peserta, p_nama_peserta, p_bib, p_tag, p_status);

    -- Ambil ID yang baru dimasukkan
    SET p_id = LAST_INSERT_ID();

    -- Commit transaksi jika berhasil
    COMMIT;

    -- Set output hasil sukses
    SET p_error_message = 'Y';

END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for update_peserta
-- ----------------------------
DROP PROCEDURE IF EXISTS `update_peserta`;
delimiter ;;
CREATE PROCEDURE `update_peserta`(OUT p_error_message VARCHAR(255),
    IN p_id INT,
    IN p_nomor_peserta VARCHAR(25),
    IN p_nama_peserta VARCHAR(100),
    IN p_bib VARCHAR(5),
    IN p_tag VARCHAR(10),
    IN p_status VARCHAR(50))
BEGIN
    -- Menangani kesalahan menggunakan deklarasi handler
    DECLARE EXIT HANDLER FOR SQLEXCEPTION 
    BEGIN
        -- Rollback jika ada error
        ROLLBACK;
        SET p_error_message = 'Terjadi kesalahan saat update data';
    END;

    -- Inisialisasi nilai awal
    SET p_error_message = NULL;

    -- Mulai transaksi
    START TRANSACTION;

    -- Insert data peserta (id otomatis karena AUTO_INCREMENT)
    UPDATE peserta SET 
		nomor_peserta = p_nomor_peserta, 
		nama_peserta = p_nama_peserta, 
		bib = p_bib, 
		tag =p_tag 
		WHERE id = p_id;

    
    -- Commit transaksi jika berhasil
    COMMIT;

    -- Set output hasil sukses
    SET p_error_message = 'Y';

END
;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;
