-- phpMyAdmin SQL Dump
-- version 4.0.2
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Erstellungszeit: 10. Mai 2015 um 10:43
-- Server Version: 5.5.16
-- PHP-Version: 5.3.8

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Datenbank: `rotmgprod`
--
CREATE DATABASE IF NOT EXISTS `rotmgprod` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `rotmgprod`;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `accounts`
--

CREATE TABLE IF NOT EXISTS `accounts` (
  `id` bigint(255) NOT NULL AUTO_INCREMENT,
  `uuid` varchar(128) NOT NULL,
  `password` varchar(256) NOT NULL,
  `name` varchar(64) NOT NULL DEFAULT 'DEFAULT',
  `rank` int(1) NOT NULL DEFAULT '0',
  `namechosen` tinyint(1) NOT NULL DEFAULT '0',
  `verified` tinyint(1) NOT NULL DEFAULT '1',
  `guild` int(11) NOT NULL,
  `guildRank` int(11) NOT NULL,
  `guildFame` int(11) NOT NULL DEFAULT '0',
  `lastip` varchar(128) NOT NULL DEFAULT '',
  `vaultCount` int(11) NOT NULL DEFAULT '1',
  `maxCharSlot` int(11) NOT NULL DEFAULT '2',
  `regTime` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `guest` tinyint(1) NOT NULL DEFAULT '0',
  `banned` tinyint(1) NOT NULL DEFAULT '0',
  `publicMuledump` int(1) NOT NULL DEFAULT '1',
  `muted` tinyint(1) NOT NULL,
  `prodAcc` tinyint(1) NOT NULL DEFAULT '0',
  `locked` varchar(512) NOT NULL,
  `ignored` varchar(512) NOT NULL,
  `gifts` varchar(10000) NOT NULL DEFAULT '',
  `isAgeVerified` tinyint(1) NOT NULL DEFAULT '0',
  `petYardType` int(11) NOT NULL DEFAULT '1',
  `ownedSkins` varchar(2048) NOT NULL,
  `authToken` varchar(128) NOT NULL DEFAULT '',
  `acceptedNewTos` tinyint(1) NOT NULL DEFAULT '1',
  `lastSeen` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `accountInUse` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`,`uuid`,`guild`,`lastip`,`banned`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `arenalb`
--

CREATE TABLE IF NOT EXISTS `arenalb` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `wave` int(11) NOT NULL,
  `accid` int(11) NOT NULL,
  `charid` int(11) NOT NULL,
  `petid` int(11) DEFAULT NULL,
  `time` varchar(256) NOT NULL,
  `date` datetime NOT NULL,
  PRIMARY KEY (`id`,`wave`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `backpacks`
--

CREATE TABLE IF NOT EXISTS `backpacks` (
  `accId` int(11) NOT NULL,
  `charId` int(11) NOT NULL,
  `items` varchar(128) NOT NULL DEFAULT '-1, -1, -1, -1, -1, -1, -1, -1',
  PRIMARY KEY (`accId`,`charId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `boards`
--

CREATE TABLE IF NOT EXISTS `boards` (
  `guildId` int(11) NOT NULL,
  `text` varchar(1024) NOT NULL,
  PRIMARY KEY (`guildId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `characters`
--

CREATE TABLE IF NOT EXISTS `characters` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `accId` int(11) NOT NULL,
  `charId` int(11) NOT NULL,
  `charType` int(11) NOT NULL DEFAULT '782',
  `level` int(11) NOT NULL DEFAULT '1',
  `exp` int(11) NOT NULL DEFAULT '0',
  `fame` int(11) NOT NULL DEFAULT '0',
  `items` varchar(128) NOT NULL DEFAULT '-1, -1, -1, -1',
  `hpPotions` int(11) NOT NULL DEFAULT '0',
  `mpPotions` int(11) NOT NULL DEFAULT '0',
  `hp` int(11) NOT NULL DEFAULT '1',
  `mp` int(11) NOT NULL DEFAULT '1',
  `stats` varchar(128) NOT NULL DEFAULT '1, 1, 1, 1, 1, 1, 1, 1',
  `dead` tinyint(1) NOT NULL DEFAULT '0',
  `tex1` int(11) NOT NULL DEFAULT '0',
  `tex2` int(11) NOT NULL DEFAULT '0',
  `pet` int(11) NOT NULL DEFAULT '-1',
  `petId` int(11) NOT NULL DEFAULT '-1',
  `hasBackpack` int(11) NOT NULL DEFAULT '0',
  `skin` int(11) NOT NULL DEFAULT '0',
  `xpBoosterTime` int(11) NOT NULL DEFAULT '0',
  `ldTimer` int(11) NOT NULL DEFAULT '0',
  `ltTimer` int(11) NOT NULL DEFAULT '0',
  `fameStats` varchar(512) NOT NULL,
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `deathTime` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `totalFame` int(11) NOT NULL DEFAULT '0',
  `lastSeen` datetime NOT NULL,
  `lastLocation` varchar(128) NOT NULL,
  PRIMARY KEY (`id`,`accId`,`dead`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `classstats`
--

CREATE TABLE IF NOT EXISTS `classstats` (
  `accId` int(11) NOT NULL,
  `objType` int(11) NOT NULL,
  `bestLv` int(11) NOT NULL DEFAULT '1',
  `bestFame` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`accId`,`objType`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `dailyquests`
--

CREATE TABLE IF NOT EXISTS `dailyquests` (
  `accId` int(11) NOT NULL,
  `goals` varchar(512) NOT NULL,
  `tier` int(11) NOT NULL DEFAULT '1',
  `time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`accId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `death`
--

CREATE TABLE IF NOT EXISTS `death` (
  `accId` int(11) NOT NULL,
  `chrId` int(11) NOT NULL,
  `name` varchar(64) NOT NULL DEFAULT 'DEFAULT',
  `charType` int(11) NOT NULL DEFAULT '782',
  `tex1` int(11) NOT NULL DEFAULT '0',
  `tex2` int(11) NOT NULL DEFAULT '0',
  `skin` int(11) NOT NULL DEFAULT '0',
  `items` varchar(128) NOT NULL DEFAULT '-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1',
  `fame` int(11) NOT NULL DEFAULT '0',
  `exp` int(11) NOT NULL,
  `fameStats` varchar(256) NOT NULL,
  `totalFame` int(11) NOT NULL DEFAULT '0',
  `firstBorn` tinyint(1) NOT NULL,
  `killer` varchar(128) NOT NULL,
  `time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`accId`,`chrId`,`time`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `giftcodes`
--

CREATE TABLE IF NOT EXISTS `giftcodes` (
  `code` varchar(128) NOT NULL,
  `content` varchar(512) NOT NULL,
  `accId` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `globalnews`
--

CREATE TABLE IF NOT EXISTS `globalnews` (
  `slot` int(11) NOT NULL,
  `linkType` int(11) NOT NULL,
  `title` varchar(65) NOT NULL,
  `image` text NOT NULL,
  `priority` int(11) NOT NULL,
  `linkDetail` text NOT NULL,
  `platform` varchar(128) NOT NULL DEFAULT 'kabam.com,kongregate,steam,rotmg',
  `startTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `endTime` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`slot`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `guilds`
--

CREATE TABLE IF NOT EXISTS `guilds` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(128) NOT NULL DEFAULT 'DEFAULT_GUILD',
  `members` varchar(128) NOT NULL,
  `guildFame` int(11) NOT NULL,
  `totalGuildFame` int(11) NOT NULL,
  `level` int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`,`members`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `mysteryboxes`
--

CREATE TABLE IF NOT EXISTS `mysteryboxes` (
  `id` int(11) NOT NULL,
  `title` varchar(128) NOT NULL,
  `weight` int(11) NOT NULL,
  `description` varchar(128) NOT NULL,
  `contents` text NOT NULL,
  `priceAmount` int(11) NOT NULL,
  `priceCurrency` int(11) NOT NULL,
  `image` text NOT NULL,
  `icon` text NOT NULL,
  `salePrice` int(11) NOT NULL,
  `saleCurrency` int(11) NOT NULL,
  `saleEnd` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `startTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `boxEnd` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `news`
--

CREATE TABLE IF NOT EXISTS `news` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `icon` varchar(16) NOT NULL DEFAULT 'info',
  `title` varchar(128) NOT NULL DEFAULT 'Default news title',
  `text` varchar(128) NOT NULL DEFAULT 'Default news text',
  `link` varchar(256) NOT NULL DEFAULT 'http://mmoe.net/',
  `date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`,`text`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `packages`
--

CREATE TABLE IF NOT EXISTS `packages` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(128) NOT NULL,
  `maxPurchase` int(11) NOT NULL DEFAULT '-1',
  `weight` int(11) NOT NULL DEFAULT '0',
  `contents` text NOT NULL,
  `bgUrl` varchar(512) NOT NULL,
  `price` int(11) NOT NULL,
  `quantity` int(11) NOT NULL DEFAULT '-1',
  `endDate` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `pets`
--

CREATE TABLE IF NOT EXISTS `pets` (
  `accId` int(11) NOT NULL,
  `petId` int(11) NOT NULL AUTO_INCREMENT,
  `objType` smallint(5) NOT NULL,
  `skinName` varchar(128) NOT NULL,
  `skin` int(11) NOT NULL,
  `family` int(11) NOT NULL,
  `rarity` int(11) NOT NULL,
  `maxLevel` int(11) NOT NULL DEFAULT '30',
  `abilities` varchar(128) NOT NULL,
  `levels` varchar(128) NOT NULL,
  `xp` varchar(128) NOT NULL DEFAULT '0, 0, 0',
  PRIMARY KEY (`accId`,`petId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `stats`
--

CREATE TABLE IF NOT EXISTS `stats` (
  `accId` int(11) NOT NULL,
  `fame` int(11) NOT NULL,
  `totalFame` int(11) NOT NULL,
  `credits` int(11) NOT NULL,
  `totalCredits` int(11) NOT NULL,
  `fortuneTokens` int(11) NOT NULL,
  `totalFortuneTokens` int(11) NOT NULL,
  PRIMARY KEY (`accId`,`fame`,`totalFame`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `thealchemist`
--

CREATE TABLE IF NOT EXISTS `thealchemist` (
  `id` int(11) NOT NULL,
  `title` varchar(512) NOT NULL,
  `description` varchar(512) DEFAULT NULL,
  `image` varchar(512) NOT NULL,
  `icon` varchar(512) NOT NULL,
  `contents` text NOT NULL,
  `priceFirstInGold` int(11) NOT NULL DEFAULT '51',
  `priceFirstInToken` int(11) NOT NULL DEFAULT '1',
  `priceSecondInGold` int(11) NOT NULL DEFAULT '75',
  `startTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `endTime` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `unlockedclasses`
--

CREATE TABLE IF NOT EXISTS `unlockedclasses` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `accId` int(11) NOT NULL,
  `class` varchar(128) NOT NULL,
  `available` varchar(128) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `vaults`
--

CREATE TABLE IF NOT EXISTS `vaults` (
  `accId` int(11) NOT NULL,
  `chestId` int(11) NOT NULL AUTO_INCREMENT,
  `items` varchar(128) NOT NULL,
  PRIMARY KEY (`accId`,`chestId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
