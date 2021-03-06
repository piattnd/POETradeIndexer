CREATE TABLE `POE_ACCOUNT` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `ACCOUNT_NAME` varchar(60) DEFAULT NULL,
  `LAST_CHAR_NAME` varchar(60) DEFAULT NULL,
  `LAST_ITEM_ADDED` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `idx_POE_ACCOUNTS_ACCOUNT_NAME_ID` (`ACCOUNT_NAME`,`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `POE_CHANGE` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `NEXT_CHANGE_ID` varchar(256) NOT NULL,
  `CHANGE_RECEIVED` datetime DEFAULT NULL,
  `STASH_COUNT` int(11) DEFAULT NULL,
  `JSON_DATA` mediumtext,
  `PROCESSED` tinyint(4) DEFAULT NULL,
  `JSON_DATA_RETRIEVED` tinyint(4) DEFAULT NULL,
  `CHANGE_PROCESSED` datetime DEFAULT NULL,
  `IN_PROCESS` tinyint(4) DEFAULT NULL,
  `PROCESS_START` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `idx_POE_CHANGE_PROCESSED_JSON_DATA_RETRIEVED` (`PROCESSED`,`JSON_DATA_RETRIEVED`),
  KEY `idx_POE_CHANGE_NEXT_CHANGE_ID` (`NEXT_CHANGE_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;


CREATE TABLE `POE_STASH` (
  `STASH_UNIQUE_ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `ID` varchar(256) NOT NULL,
  `STASH_NAME` varchar(100) DEFAULT NULL,
  `STASH_TYPE` varchar(45) DEFAULT NULL,
  `ACCOUNT_ID` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`STASH_UNIQUE_ID`),
  KEY `stashToAcctId` (`ACCOUNT_ID`),
  KEY `idx_POE_STASH_ID` (`ID`),
  CONSTRAINT `poe_stash_ibfk_1` FOREIGN KEY (`ACCOUNT_ID`) REFERENCES `POE_ACCOUNT` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `POE_ITEM` (
  `UNIQUE_ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `ID` varchar(256) NOT NULL,
  `STASH_ID` bigint(20) DEFAULT NULL,
  `ITEM_NAME` varchar(100) DEFAULT NULL,
  `VERIFIED` tinyint(4) DEFAULT NULL,
  `ITEM_LEVEL` int(11) DEFAULT NULL,
  `TYPE_LINE` varchar(100) DEFAULT NULL,
  `IDENTIFIED` tinyint(4) DEFAULT NULL,
  `CORRUPTED` tinyint(4) DEFAULT NULL,
  `LOCKED_TO_CHAR` tinyint(4) DEFAULT NULL,
  `NOTE` longtext,
  `FRAME_TYPE` smallint(6) DEFAULT NULL,
  `STASH_POS_X` int(11) DEFAULT NULL,
  `STASH_POS_Y` int(11) DEFAULT NULL,
  `SEC_DESCRIPTION` longtext,
  `DESCRIPTION_TEXT` longtext,
  `DUPLICATED` tinyint(4) DEFAULT NULL,
  `MAX_STACK_SIZE` int(11) DEFAULT NULL,
  `STACK_SIZE` int(11) DEFAULT NULL,
  `SUPPORT` tinyint(4) DEFAULT NULL,
  `SLOT_WIDTH` int(11) DEFAULT NULL,
  `SLOT_HEIGHT` int(11) DEFAULT NULL,
  `ICON` mediumtext,
  `LEAGUE` varchar(45) DEFAULT NULL,
  `INVENTORY_ID` varchar(256) DEFAULT NULL,
  `ART_FILE_NAME` varchar(256) DEFAULT NULL,
  `TALISMAN_TIER` tinyint(4) DEFAULT NULL,
  `PROPH_DIFF_TEXT` varchar(256) DEFAULT NULL,
  `PROPHECY_TEXT` mediumtext,
  `IS_RELIC` tinyint(4) DEFAULT NULL,
  `SOCKETED_TO_ITEM` bigint(20) DEFAULT NULL,
  `ADDED_TS` datetime DEFAULT NULL,
  `CONCAT_FLAVOUR_TEXT` text,
  `CHANGE_ID` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`UNIQUE_ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`),
  KEY `idx_POE_ITEM_SOCKETED_TO_ITEM` (`SOCKETED_TO_ITEM`),
  KEY `stashIdConstraint` (`STASH_ID`),
  KEY `POE_ITEM` (`CHANGE_ID`),
  CONSTRAINT `poe_item_ibfk_1` FOREIGN KEY (`STASH_ID`) REFERENCES `POE_STASH` (`STASH_UNIQUE_ID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `poe_item_ibfk_3` FOREIGN KEY (`CHANGE_ID`) REFERENCES `POE_CHANGE` (`ID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `POE_MODS` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `ITEM_ID` bigint(20) DEFAULT NULL,
  `MOD_TEXT` text,
  `MOD_TYPE` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `itemToMod` (`ITEM_ID`),
  CONSTRAINT `poe_mods_ibfk_1` FOREIGN KEY (`ITEM_ID`) REFERENCES `POE_ITEM` (`UNIQUE_ID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `POE_PROPERTY` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `ITEM_ID` bigint(20) DEFAULT NULL,
  `DISPLAY_MODE` int(11) DEFAULT NULL,
  `TYPE` int(11) DEFAULT NULL,
  `PROGRESS` int(11) DEFAULT NULL,
  `IS_ADDITIONAL_PROPERTY` tinyint(4) DEFAULT NULL,
  `PROPERTY_VALUE` int(11) DEFAULT NULL,
  `PROPERTY_TYPE_ID` bigint(20) DEFAULT NULL,
  `PROPERTY_VALUE_STRING` varchar(100) DEFAULT NULL,
  `PROPERTY_NAME` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `idx_POE_PROPERTY_ITEM_ID` (`ITEM_ID`),
  KEY `propTypeConstraint` (`PROPERTY_TYPE_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `POE_REQUIREMENT` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `ITEM_ID` bigint(20) DEFAULT NULL,
  `DISPLAY_MODE` tinyint(4) DEFAULT NULL,
  `TYPE` tinyint(4) DEFAULT NULL,
  `PROGRESS` tinyint(4) DEFAULT NULL,
  `IS_ADDITIONAL_REQUIREMENT` tinyint(4) DEFAULT NULL,
  `REQUIREMENT_VALUE` int(11) DEFAULT NULL,
  `REQUIREMENT_TYPE_ID` bigint(20) DEFAULT NULL,
  `REQUIREMENT_VALUE_STRING` varchar(45) DEFAULT NULL,
  `IS_NEXT_LEVEL_REQUIREMENT` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `idx_POE_REQUIREMENT_ITEM_ID` (`ITEM_ID`),
  KEY `reqTypeConstraint` (`REQUIREMENT_TYPE_ID`),
  CONSTRAINT `poe_requirement_ibfk_1` FOREIGN KEY (`ITEM_ID`) REFERENCES `POE_ITEM` (`UNIQUE_ID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `POE_SOCKET` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `ITEM_ID` bigint(20) DEFAULT NULL,
  `GROUP_ID` int(11) DEFAULT NULL,
  `ATTRIBUTE` varchar(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `idx_POE_SOCKET_ITEM_ID` (`ITEM_ID`),
  CONSTRAINT `poe_socket_ibfk_1` FOREIGN KEY (`ITEM_ID`) REFERENCES `POE_ITEM` (`UNIQUE_ID`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
