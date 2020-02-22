CREATE DATABASE `cigars` /*!40100 COLLATE 'utf8_general_ci' */;

CREATE TABLE `brands` (
	`uid` VARCHAR(50) NOT NULL,
	`name` VARCHAR(50) NULL DEFAULT NULL
)
COLLATE='utf8_general_ci'
ENGINE=InnoDB
;


CREATE TABLE `items` (
	`ID` VARCHAR(50) NOT NULL,
	`CIGAR` VARCHAR(100) NULL DEFAULT NULL,
	`LENGTH` VARCHAR(50) NULL DEFAULT NULL,
	`RING` VARCHAR(50) NULL DEFAULT NULL,
	`COUNTRY` VARCHAR(50) NULL DEFAULT NULL,
	`FILLER` VARCHAR(50) NULL DEFAULT NULL,
	`WRAPPER` VARCHAR(50) NULL DEFAULT NULL,
	`COLOR` VARCHAR(50) NULL DEFAULT NULL,
	`STRENGTH` VARCHAR(50) NULL DEFAULT NULL
)
COLLATE='utf8_general_ci'
ENGINE=InnoDB
;
