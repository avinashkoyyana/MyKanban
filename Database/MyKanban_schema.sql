CREATE DATABASE  IF NOT EXISTS `mykanban` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `mykanban`;
-- MySQL dump 10.13  Distrib 5.6.17, for Win32 (x86)
--
-- Host: 127.0.0.1    Database: mykanban
-- ------------------------------------------------------
-- Server version	5.6.19

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `_dont_use_board_sprint`
--

DROP TABLE IF EXISTS `_dont_use_board_sprint`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `_dont_use_board_sprint` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique internal key for this board sprint entry',
  `board_id` bigint(20) DEFAULT NULL COMMENT 'ID# of parent board',
  `sequence` int(11) DEFAULT NULL COMMENT 'Ordinal position of this sprint within list of all sprints for this board',
  `start_date` date DEFAULT NULL COMMENT 'Start date for this sprint',
  `end_date` date DEFAULT NULL COMMENT 'End date for this sprint',
  PRIMARY KEY (`id`),
  KEY `board_id_idx` (`board_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Contains one row for each sprint for a given board.  Note that there is always a virtual sprint "0" for any board that represents a custom date range for filtering, but that may not be assigned to a task.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `_dont_use_board_status`
--

DROP TABLE IF EXISTS `_dont_use_board_status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `_dont_use_board_status` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `board_id` bigint(20) DEFAULT NULL COMMENT 'ID# of parent board',
  `column_heading` varchar(20) DEFAULT NULL COMMENT 'Text to display at top of column',
  `status` varchar(50) DEFAULT NULL COMMENT 'Status value to store for tasks in this column on this board',
  `sequence` tinyint(4) DEFAULT NULL COMMENT 'Ordinal position of this column within this board',
  PRIMARY KEY (`id`),
  KEY `board_id_idx` (`board_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Contains one row for each status (i.e. column) that appears on a board.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `_dont_use_comment`
--

DROP TABLE IF EXISTS `_dont_use_comment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `_dont_use_comment` (
  `id` bigint(20) NOT NULL COMMENT 'Unique key for this comment',
  `task_id` bigint(20) DEFAULT NULL COMMENT 'Task this comment is associated with',
  `person_id` bigint(20) DEFAULT NULL COMMENT 'ID# of person entering this comment',
  `date_time_added` datetime DEFAULT NULL COMMENT 'Date and time comment added',
  `comment` varchar(1000) DEFAULT NULL COMMENT 'Text of comment',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Contains one row for each comment associated with a task';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `board`
--

DROP TABLE IF EXISTS `board`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `board` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique internal key for a board',
  `name` varchar(100) DEFAULT NULL COMMENT 'Display name for the board',
  `board_set_id` bigint(20) DEFAULT NULL,
  `created` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified` datetime DEFAULT CURRENT_TIMESTAMP,
  `created_by` bigint(20) DEFAULT '0',
  `modified_by` bigint(20) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `idx_board_board_set_id` (`board_set_id`),
  KEY `NAME` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=3042 DEFAULT CHARSET=utf8 COMMENT='The "Board" table contains one row for each board contained in the system.  A board is a set of columns that a task can be moved between indicating the status of a task.  Tasks can appear on more than one board, if the task''s parent project in included in more than one board.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `board_person`
--

DROP TABLE IF EXISTS `board_person`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `board_person` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `board_id` bigint(20) DEFAULT NULL COMMENT 'ID# of board',
  `person_id` bigint(20) DEFAULT NULL COMMENT 'ID# of person in this board',
  `can_add` tinyint(1) DEFAULT '0',
  `can_edit` tinyint(1) DEFAULT '0',
  `can_delete` tinyint(1) DEFAULT '0',
  `can_read` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `board_id_idx` (`board_id`),
  KEY `person_id_idx` (`person_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3148 DEFAULT CHARSET=utf8 COMMENT='Contains one row for each person who is associated with a board.  This table controls the permissions the individual has on this board.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `board_project`
--

DROP TABLE IF EXISTS `board_project`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `board_project` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `board_id` bigint(20) DEFAULT NULL,
  `project_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_project` (`project_id`),
  KEY `idx_board` (`board_id`)
) ENGINE=InnoDB AUTO_INCREMENT=7890 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `board_set`
--

DROP TABLE IF EXISTS `board_set`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `board_set` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  `created` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified` datetime DEFAULT CURRENT_TIMESTAMP,
  `created_by` bigint(20) DEFAULT '0',
  `modified_by` bigint(20) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `idx_board_set_name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=1062 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `board_set_status`
--

DROP TABLE IF EXISTS `board_set_status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `board_set_status` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `board_set_id` bigint(20) DEFAULT NULL COMMENT 'ID# of parent board',
  `column_heading` varchar(20) DEFAULT NULL COMMENT 'Text to display at top of column',
  `status` varchar(100) DEFAULT NULL COMMENT 'Status value to store for tasks in this column on this board',
  `sequence` int(11) DEFAULT NULL COMMENT 'Ordinal position of this column within this board',
  `created_by` bigint(20) DEFAULT '0',
  `created` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` bigint(20) DEFAULT '0',
  `modified` datetime DEFAULT CURRENT_TIMESTAMP,
  `fore_color` varchar(45) DEFAULT 'black',
  `back_color` varchar(45) DEFAULT 'white',
  PRIMARY KEY (`id`),
  KEY `board_id_idx` (`board_set_id`),
  KEY `idx_board_set_status_status` (`status`)
) ENGINE=InnoDB AUTO_INCREMENT=2361 DEFAULT CHARSET=utf8 COMMENT='Contains one row for each status (i.e. column) that appears on a board.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `board_sprint`
--

DROP TABLE IF EXISTS `board_sprint`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `board_sprint` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique internal key for this board sprint entry',
  `board_id` bigint(20) DEFAULT NULL COMMENT 'ID# of parent board',
  `sequence` int(11) DEFAULT NULL COMMENT 'Ordinal position of this sprint within list of all sprints for this board',
  `start_date` date DEFAULT NULL COMMENT 'Start date for this sprint',
  `end_date` date DEFAULT NULL COMMENT 'End date for this sprint',
  PRIMARY KEY (`id`),
  KEY `board_id_idx` (`board_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Contains one row for each sprint for a given board.  Note that there is always a virtual sprint "0" for any board that represents a custom date range for filtering, but that may not be assigned to a task.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `board_task`
--

DROP TABLE IF EXISTS `board_task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `board_task` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `board_id` bigint(20) DEFAULT NULL COMMENT 'ID# of board this task is associated with',
  `task_id` bigint(20) DEFAULT NULL COMMENT 'ID# of task',
  `sequence` int(11) DEFAULT NULL COMMENT 'The ordinal position of this task within its current column on the board',
  PRIMARY KEY (`id`),
  KEY `board_id_idx` (`board_id`),
  KEY `task_id_idx` (`task_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Contains one row for each task associated with a board.  In particular, this table is used to store the ordinal position of a task with in a board column.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `comment`
--

DROP TABLE IF EXISTS `comment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `comment` (
  `id` bigint(20) NOT NULL COMMENT 'Unique key for this comment',
  `task_id` bigint(20) DEFAULT NULL COMMENT 'Task this comment is associated with',
  `person_id` bigint(20) DEFAULT NULL COMMENT 'ID# of person entering this comment',
  `date_time_added` datetime DEFAULT NULL COMMENT 'Date and time comment added',
  `comment` varchar(1000) DEFAULT NULL COMMENT 'Text of comment',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Contains one row for each comment associated with a task';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `person`
--

DROP TABLE IF EXISTS `person`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `person` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique internal key for this person',
  `name` varchar(100) DEFAULT NULL COMMENT 'Display name of this person',
  `picture_url` varchar(255) DEFAULT NULL COMMENT 'Url to individual''s picture',
  `email` varchar(100) DEFAULT NULL COMMENT 'Email address for this person',
  `phone` varchar(100) DEFAULT NULL COMMENT 'Phone # of this person',
  `user_name` varchar(100) DEFAULT NULL COMMENT 'Unique user name of this person (if they can log in)',
  `password` varchar(100) DEFAULT NULL COMMENT 'Encrypted password for this user (if they can log in)',
  `can_login` tinyint(4) DEFAULT '0' COMMENT '0=no, 1=yes',
  `created` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified` datetime DEFAULT CURRENT_TIMESTAMP,
  `created_by` bigint(20) DEFAULT '0',
  `modified_by` bigint(20) DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `user_name_UNIQUE` (`user_name`),
  KEY `idx_person_name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=5980 DEFAULT CHARSET=utf8 COMMENT='Contains one row for each person who may use or be referred to by the system.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `project`
--

DROP TABLE IF EXISTS `project`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `project` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID# of this project',
  `name` varchar(100) DEFAULT NULL COMMENT 'Display name for this project',
  `project_lead` bigint(20) DEFAULT NULL COMMENT 'ID# of user who is the project lead',
  `expected_start_date` date DEFAULT NULL COMMENT 'Date project is expected to start',
  `expected_end_date` date DEFAULT NULL COMMENT 'Date project is expected to end',
  `earliest_task_start_date` date DEFAULT NULL COMMENT 'Earliest start date for all tasks associated with this project',
  `latest_task_end_date` date DEFAULT NULL COMMENT 'Latest end date for all tasks associated with this project',
  `define_done` varchar(1000) DEFAULT NULL COMMENT 'Definition of done for this project',
  `task_est_hours` decimal(10,2) DEFAULT NULL COMMENT 'Sum of estimated hours for all tasks in project',
  `task_act_hours` decimal(10,2) DEFAULT NULL COMMENT 'Sum of all actual hours for tasks in project',
  `status` bigint(20) DEFAULT NULL,
  `board_set` bigint(20) DEFAULT NULL COMMENT 'The set of boards this project may be added to.',
  `created` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified` datetime DEFAULT CURRENT_TIMESTAMP,
  `created_by` bigint(20) DEFAULT '0',
  `modified_by` bigint(20) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `idx_project_name` (`name`),
  KEY `idx_project_board_set` (`board_set`),
  KEY `idx_project_status` (`status`)
) ENGINE=InnoDB AUTO_INCREMENT=2868 DEFAULT CHARSET=utf8 COMMENT='Contains one row for each project in the system.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `project_stakeholder`
--

DROP TABLE IF EXISTS `project_stakeholder`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `project_stakeholder` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique internal key that identifies this project-stakeholder relationship',
  `project_id` bigint(20) DEFAULT NULL COMMENT 'ID# of parent project',
  `person_id` bigint(20) DEFAULT NULL COMMENT 'ID# of person',
  `role` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `project_id_idx` (`project_id`),
  KEY `person_id_idx` (`person_id`)
) ENGINE=InnoDB AUTO_INCREMENT=172 DEFAULT CHARSET=utf8 COMMENT='Contains one row for each individual who has a stake in the outcome of this project.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `property`
--

DROP TABLE IF EXISTS `property`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `property` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  `value` varchar(1000) DEFAULT NULL,
  `parent_type` varchar(100) DEFAULT NULL,
  `parent_id` bigint(20) DEFAULT NULL,
  `created` datetime DEFAULT CURRENT_TIMESTAMP,
  `created_by` bigint(20) DEFAULT '0',
  `modified` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` bigint(20) DEFAULT '0',
  `value_type` varchar(100) DEFAULT 'string',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=76 DEFAULT CHARSET=utf8 COMMENT='Stores a property associated with a given object';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sprint`
--

DROP TABLE IF EXISTS `sprint`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sprint` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique internal key for this board sprint entry',
  `board_id` bigint(20) DEFAULT NULL COMMENT 'ID# of parent board',
  `sequence` int(11) DEFAULT NULL COMMENT 'Ordinal position of this sprint within list of all sprints for this board',
  `start_date` date DEFAULT NULL COMMENT 'Start date for this sprint',
  `end_date` date DEFAULT NULL COMMENT 'End date for this sprint',
  `created` datetime DEFAULT CURRENT_TIMESTAMP,
  `created_by` bigint(20) DEFAULT '0',
  `modified` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` bigint(20) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `board_id_idx` (`board_id`)
) ENGINE=InnoDB AUTO_INCREMENT=638 DEFAULT CHARSET=utf8 COMMENT='Contains one row for each sprint for a given board.  Note that there is always a virtual sprint "0" for any board that represents a custom date range for filtering, but that may not be assigned to a task.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tag`
--

DROP TABLE IF EXISTS `tag`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tag` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique internal key for this tag',
  `tag` varchar(100) DEFAULT NULL COMMENT 'Display text of tag',
  `created` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified` datetime DEFAULT CURRENT_TIMESTAMP,
  `created_by` bigint(20) DEFAULT '0',
  `modified_by` bigint(20) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `tag_idx` (`tag`)
) ENGINE=InnoDB AUTO_INCREMENT=533 DEFAULT CHARSET=utf8 COMMENT='Contains one row for each unique tag in the system';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task`
--

DROP TABLE IF EXISTS `task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique internal ID # for this task',
  `project_id` bigint(20) DEFAULT NULL COMMENT 'ID# of the project this task belongs to',
  `name` varchar(100) DEFAULT NULL COMMENT 'Display name for this task',
  `start_date` date DEFAULT NULL COMMENT 'Start date for this task',
  `end_date` date DEFAULT NULL COMMENT 'End date for this task',
  `define_done` varchar(1000) DEFAULT NULL COMMENT 'Description of done for this task',
  `status_id` bigint(20) DEFAULT NULL COMMENT 'ID# of the board status for this task',
  `est_hours` decimal(10,2) DEFAULT NULL COMMENT 'Estimated hours to complete this task',
  `act_hours` decimal(10,2) DEFAULT NULL COMMENT 'Actual hours to complete this task',
  `parent_task_id` bigint(20) DEFAULT NULL COMMENT 'ID# of parent task, if any',
  `sub_task_est_hours` decimal(10,2) DEFAULT NULL COMMENT 'Sum of estimated hours for all sub-tasks (if any)',
  `sub_task_act_hours` double(10,2) DEFAULT NULL COMMENT 'Sum of actual hours for all sub-tasks (if any)',
  `created` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified` datetime DEFAULT CURRENT_TIMESTAMP,
  `created_by` bigint(20) DEFAULT '0',
  `modified_by` bigint(20) DEFAULT '0',
  `sequence` int(11) DEFAULT '999',
  PRIMARY KEY (`id`),
  KEY `project_id_idx` (`project_id`),
  KEY `parent_task_id_idx` (`parent_task_id`),
  KEY `idx_task_status_id` (`status_id`),
  KEY `idx_sequence` (`sequence`)
) ENGINE=InnoDB AUTO_INCREMENT=7759 DEFAULT CHARSET=utf8 COMMENT='Contains one row for each task that may be associated with a project.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_approver`
--

DROP TABLE IF EXISTS `task_approver`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_approver` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique internal key for this task requestor',
  `task_id` bigint(20) DEFAULT NULL COMMENT 'ID# of task this requestor is associated with',
  `person_id` bigint(20) DEFAULT NULL COMMENT 'ID# of person who requested this task',
  PRIMARY KEY (`id`),
  KEY `task_id_idx` (`task_id`),
  KEY `person_id_idx` (`person_id`)
) ENGINE=InnoDB AUTO_INCREMENT=153 DEFAULT CHARSET=utf8 COMMENT='Contains one row for each approver associated with a task';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_assignee`
--

DROP TABLE IF EXISTS `task_assignee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_assignee` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `task_id` bigint(20) DEFAULT NULL COMMENT 'ID# of task this person is assigned to',
  `person_id` bigint(20) DEFAULT NULL COMMENT 'ID# of person who is assigned to this task',
  `role` varchar(45) DEFAULT NULL,
  `est_hours` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `task_id` (`task_id`),
  KEY `person_id` (`person_id`)
) ENGINE=InnoDB AUTO_INCREMENT=438 DEFAULT CHARSET=utf8 COMMENT='Contains one row for each assignee associated with a task';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_comment`
--

DROP TABLE IF EXISTS `task_comment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_comment` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique key for this comment',
  `task_id` bigint(20) DEFAULT NULL COMMENT 'Task this comment is associated with',
  `comment` varchar(1000) DEFAULT NULL COMMENT 'Text of comment',
  `created` datetime DEFAULT CURRENT_TIMESTAMP,
  `created_by` bigint(20) DEFAULT '0',
  `modified` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` varchar(45) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `idx_task_id` (`task_id`)
) ENGINE=InnoDB AUTO_INCREMENT=374 DEFAULT CHARSET=utf8 COMMENT='Contains one row for each comment associated with a task';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_person_preference`
--

DROP TABLE IF EXISTS `task_person_preference`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_person_preference` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique internal key for this entry',
  `task_id` bigint(20) DEFAULT NULL COMMENT 'ID# of task',
  `person_id` bigint(20) DEFAULT NULL COMMENT 'ID# of person',
  `is_collapsed` tinyint(4) DEFAULT '0' COMMENT '0=no, 1=yes',
  PRIMARY KEY (`id`),
  KEY `task_id_idx` (`task_id`),
  KEY `person_id_idx` (`person_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Used to store person-specific task preferences.  Specifically whether a task should be expanded or collapsed on the board.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_requestor`
--

DROP TABLE IF EXISTS `task_requestor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_requestor` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique internal key for this task requestor',
  `task_id` bigint(20) DEFAULT NULL COMMENT 'ID# of task this requestor is associated with',
  `person_id` bigint(20) DEFAULT NULL COMMENT 'ID# of person who requested this task',
  PRIMARY KEY (`id`),
  KEY `task_id_idx` (`task_id`),
  KEY `person_id_idx` (`person_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Contains one row for each requestor associated with a task';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_tag`
--

DROP TABLE IF EXISTS `task_tag`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_tag` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'Unique internal key for this tag instance',
  `task_id` bigint(20) DEFAULT NULL COMMENT 'ID# of task this tag is associated with',
  `tag_id` bigint(20) DEFAULT NULL COMMENT 'ID# of tag to associate with this task',
  `created` datetime DEFAULT CURRENT_TIMESTAMP,
  `created_by` bigint(20) DEFAULT '0',
  `modified` datetime DEFAULT CURRENT_TIMESTAMP,
  `modified_by` bigint(20) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `task_id_idx` (`task_id`),
  KEY `tag_id_idx` (`tag_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2556 DEFAULT CHARSET=utf8 COMMENT='Contains one row for each tag associated with a task';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary table structure for view `vw_board`
--

DROP TABLE IF EXISTS `vw_board`;
/*!50001 DROP VIEW IF EXISTS `vw_board`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `vw_board` (
  `id` tinyint NOT NULL,
  `name` tinyint NOT NULL,
  `board_set_id` tinyint NOT NULL
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Temporary table structure for view `vw_person`
--

DROP TABLE IF EXISTS `vw_person`;
/*!50001 DROP VIEW IF EXISTS `vw_person`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `vw_person` (
  `id` tinyint NOT NULL,
  `name` tinyint NOT NULL,
  `picture_url` tinyint NOT NULL,
  `email` tinyint NOT NULL,
  `phone` tinyint NOT NULL,
  `user_name` tinyint NOT NULL,
  `password` tinyint NOT NULL,
  `can_login` tinyint NOT NULL
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Temporary table structure for view `vw_project`
--

DROP TABLE IF EXISTS `vw_project`;
/*!50001 DROP VIEW IF EXISTS `vw_project`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `vw_project` (
  `id` tinyint NOT NULL,
  `name` tinyint NOT NULL,
  `project_lead` tinyint NOT NULL,
  `expected_start_date` tinyint NOT NULL,
  `expected_end_date` tinyint NOT NULL,
  `earliest_task_start_date` tinyint NOT NULL,
  `latest_task_end_date` tinyint NOT NULL,
  `status` tinyint NOT NULL,
  `define_done` tinyint NOT NULL,
  `task_est_hours` tinyint NOT NULL,
  `task_act_hours` tinyint NOT NULL
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Temporary table structure for view `vw_tag`
--

DROP TABLE IF EXISTS `vw_tag`;
/*!50001 DROP VIEW IF EXISTS `vw_tag`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `vw_tag` (
  `id` tinyint NOT NULL,
  `tag` tinyint NOT NULL
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Temporary table structure for view `vw_task`
--

DROP TABLE IF EXISTS `vw_task`;
/*!50001 DROP VIEW IF EXISTS `vw_task`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `vw_task` (
  `id` tinyint NOT NULL,
  `project_id` tinyint NOT NULL,
  `name` tinyint NOT NULL,
  `start_date` tinyint NOT NULL,
  `end_date` tinyint NOT NULL,
  `define_done` tinyint NOT NULL,
  `status_id` tinyint NOT NULL,
  `est_hours` tinyint NOT NULL,
  `act_hours` tinyint NOT NULL,
  `parent_task_id` tinyint NOT NULL,
  `sub_task_est_hours` tinyint NOT NULL,
  `sub_task_act_hours` tinyint NOT NULL
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Dumping routines for database 'mykanban'
--
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_approver_to_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_add_approver_to_task`(
	t_task_id bigint, 
	t_person_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a new approver to a given task

	Params:		t_task_id ..... ID# of the task to add approver to	
				t_person_id ... ID# of the person to add as an approver
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SET @cnt = (SELECT COUNT(*) FROM task_approver WHERE task_id = t_task_id AND person_id = t_person_id);
	IF (@cnt = 0) THEN
		INSERT INTO task_approver (task_id, person_id) VALUES (t_task_id, t_person_id);
	END IF;
	SELECT * FROM task_approver 
		WHERE task_id = t_task_id
		  AND person_id = t_person_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_assignee_to_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_add_assignee_to_task`(t_task_id bigint, t_person_id bigint)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a new assignee to a given task

	Params:		t_task_id ..... ID# of the task to add assignee to	
				t_person_id ... ID# of the person to add as an assignee
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SET @cnt = (SELECT COUNT(*) FROM task_assignee WHERE task_id = t_task_id AND person_id = t_person_id);
	IF (@cnt = 0) THEN
		INSERT INTO task_assignee (task_id, person_id) VALUES (t_task_id, t_person_id);
	END IF;
	SELECT @cnt;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_board` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_add_board`(
	name varchar(100),
	b_board_set_id bigint,
	b_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a new Kanban board to the system

	Params:		name ............. Name of new board
				b_board_set_id ... ID# of board set new board belongs to
				b_user_id ........ ID# of person adding this board
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	INSERT INTO `mykanban`.`board` (`name`, board_set_id, created_by, modified_by, created, modified)
	VALUES (name, b_board_set_id, b_user_id, b_user_id, now(), now());
	SELECT * FROM board WHERE id = LAST_INSERT_ID();

	-- Add the user who is creating this board to the list
	-- of board users and give full privileges
	call sp_add_person_to_board(LAST_INSERT_ID(), b_user_id, 1, 1, 1, 1);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_board_set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_add_board_set`(
	name varchar(100),
	bs_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a new board set to the system

	Params:		name ........... Name of new board set
				bs_user_id ..... ID# of person adding the board set
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	INSERT INTO `mykanban`.`board_set` (`name`, created, created_by, modified, modified_by)
	VALUES (name, now(), bs_user_id, now(), bs_user_id);

	SELECT * FROM board_set WHERE id = LAST_INSERT_ID();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_person` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_add_person`(
	p_name varchar(100),
	p_picture_url varchar(255),
	p_email varchar(100),
	p_phone varchar(100),
	p_user_name varchar(100),
	p_password varchar(100),
	p_can_login tinyint,
	p_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a new person to the system

	Params:		p_name .......... First & last name of person being added
				p_picture_url ... URL of picture of this person
				p_email ......... Email address
				p_phone ......... Phone #
				p_user_name ..... User login name (must be unique)
				p_password ...... Encrypted user password
				p_can_login ..... Indicates whether this person can login to system
				p_user_id ....... ID# of person adding this person
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	INSERT INTO `mykanban`.`person`
		(`name`,
		`picture_url`,
		`email`,
		`phone`,
		`user_name`,
		`password`,
		`can_login`,
		created_by,
		created,
		modified_by,
		modified)
	VALUES
		(p_name,
		p_picture_url,
		p_email,
		p_phone,
		p_user_name,
		p_password,
		p_can_login,
		p_user_id,
		now(),
		p_user_id,
		now()
	);

	SELECT * FROM vw_person WHERE id = LAST_INSERT_ID();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_person_to_board` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_add_person_to_board`(
	p_board_id bigint, 
	p_person_id bigint,
	p_can_add tinyint,
	p_can_edit tinyint,
	p_can_delete tinyint,
	p_can_read tinyint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Give person access to a Kanban board

	Params:		p_board_id ...... ID# of Kanban board
				p_person_id ..... ID# of person to add
				p_can_add ....... 1=user can add new tasks
				p_can_edit ...... 1=user can edit existing tasks
				p_can_delete .... 1=user can delete existing tasks
				p_can_read ...... 1=user can read (view) tasks
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SET @id = (SELECT id FROM board_person WHERE board_id = p_board_id AND person_id = p_person_id);
	IF (@id IS NULL) THEN
		INSERT INTO board_person (board_id, person_id) VALUES (p_board_id, p_person_id);
		SET @id = LAST_INSERT_ID();
	END IF;
	UPDATE board_person SET can_add = p_can_add,
		can_edit = p_can_edit,
		can_delete = p_can_delete,
		can_read = p_can_read
	WHERE id = @id;
	SELECT * FROM board_person WHERE id = @id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_project` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_add_project`(
	p_name varchar(100),
	p_project_lead bigint,
	p_expected_start_date date,
	p_expected_end_date date,
	p_earliest_task_start_date date,
	p_latest_task_end_date date,
	p_status bigint,
	p_define_done varchar(1000),
	p_task_est_hours decimal(10,2),
	p_task_act_hours decimal(10,2),
	p_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a new project to the system

	Params:		p_name ...................... Name of project to add
				p_project_lead ............... ID# of person who is project lead
				p_expected_start_date ........ Expected date project will start
				p_expected_end_date .......... Expected date project will end
				p_earliest_task_start_date ... Earliest start date of all tasks
				p_latest_task_end_date ....... Latest end date of all tasks
				p_status ..................... ID# of project status code
				p_define_done ................ Expected deliverable or outcome
				p_task_est_hours ............. Sum of all task estimated hours
				p_task_act_hours ............. Sum of all task actual hours
				p_user_id .................... ID# of person adding this project
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	INSERT INTO `mykanban`.`project`
		(`name`,
		`project_lead`,
		`expected_start_date`,
		`expected_end_date`,
		`earliest_task_start_date`,
		`latest_task_end_date`,
		`status`,
		`define_done`,
		`task_est_hours`,
		`task_act_hours`,
		created_by,
		modified_by,
		created,
		modified)
	VALUES
		(p_name,
		p_project_lead,
		p_expected_start_date,
		p_expected_end_date,
		p_earliest_task_start_date,
		p_latest_task_end_date,
		p_status,
		p_define_done,
		p_task_est_hours,
		p_task_act_hours,
		p_user_id,
		p_user_id,
		now(),
		now()
	);	

	SELECT * FROM vw_project WHERE id = LAST_INSERT_ID();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_project_to_board` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_add_project_to_board`(
	p_board_id bigint, 
	p_project_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a project to a Kanban board

	Params:		p_board_id ........ ID# of board to add project to
				p_project_id ...... ID# of project to add to board
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	DELETE FROM board_project WHERE board_id = p_board_id AND project_id = p_project_id;
	INSERT INTO board_project (board_id, project_id) VALUES (p_board_id, p_project_id);
	CALL sp_get_projects_by_board(p_board_id);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_property` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_add_property`(
	p_name varchar(100),
	p_value varchar(1000),
	p_value_type varchar(100),
	p_parent_type varchar(100),
	p_parent_id bigint,
	p_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a new property to the system

	Params:		name .............. name of property to add
				value ............. value to store for property
				value_type ........ type of data (e.g. int, string, etc.)
				parent_type ....... table of parent row (e.g. task, project, board)
				parent_id ......... ID# of row this property is associated with
				user_id ........... ID# of user adding this row
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	INSERT INTO property (name, value, value_type, parent_type, parent_id, created_by)
		VALUES (p_name, p_value, p_value_type, p_parent_type, p_parent_id, p_user_id);

	SELECT * FROM property WHERE id = LAST_INSERT_ID();

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_sprint` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_add_sprint`(
	s_board_id bigint,
	s_start_date datetime,
	s_end_date datetime,
	s_sequence int,
	s_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a new sprint to a board

	Params:		s_board_id ......... ID# of board to add sprint to
				s_start_date ....... Sprint begin date
				s_end_date ......... Sprint end date
				s_sequence ......... Ordinal position of sprint
				s_user_id .......... ID# of person adding this sprint
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	INSERT INTO `mykanban`.`sprint`
		(`board_id`,
		`start_date`,
		`end_date`,
		`sequence`,
		created_by,
		modified_by,
		created,
		modified)
	VALUES
		(s_board_id,
		s_start_date,
		s_end_date,
		s_sequence,
		s_user_id,
		s_user_id,
		now(),
		now()
	);

	SELECT * FROM sprint WHERE id = LAST_INSERT_ID();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_stakeholder_to_project` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_add_stakeholder_to_project`(
	p_project_id bigint, 
	p_person_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a new stakeholder to a project

	Params:		p_project_id ...... ID# of project to add stakeholder to
				p_person_id ....... ID# of person to add to project
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SET @cnt = (SELECT COUNT(*) FROM project_stakeholder WHERE project_id = p_project_id AND person_id = p_person_id);
	IF (@cnt = 0) THEN
		INSERT INTO project_stakeholder (project_id, person_id) VALUES (p_project_id, p_person_id);
	END IF;
	SELECT @cnt;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_status_code` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_add_status_code`(
	s_board_set_id bigint,
	s_column_heading varchar(20),
	s_status varchar(100),
	s_sequence int,
	s_user_id bigint,
	s_fore_color varchar(45),
	s_back_color varchar(45)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a new status code to a board set

	Params:		s_board_set_id .... Board set to add this status code to
				s_column_heading .. Display heading on Kanban boards for this status
				s_status .......... Internal name of this status
				s_sequence ........ Ordinal position of this status
				s_user_id ......... ID# of person adding this status
				s_fore_color ...... Foreground color to use when displaying tasks with this status
				s_back_color ...... Background color to use when displaying tasks with this status
	---------------------------------------------------------------------------
	Mods:		1/18/15: M.Gerow; Prevent codes without a valid board set id from being saved.
	---------------------------------------------------------------------------
*/
	IF (s_board_set_id > 0) THEN

		INSERT INTO board_set_status
			(board_set_id, column_heading, status, sequence, created_by, modified_by, created, modified,
				fore_color, back_color)
		VALUES
			(s_board_set_id, s_column_heading, s_status, s_sequence, s_user_id, s_user_id, now(), now(),
				s_fore_color, s_back_color);

		SELECT * FROM board_set_status WHERE id = LAST_INSERT_ID();

	END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_add_task`(
	t_project_id bigint,
	t_name varchar(100),
	t_start_date date,
	t_end_date date,
	t_define_done varchar(1000),
	t_status_id bigint,
	t_est_hours decimal(10,2),
	t_act_hours decimal(10,2),
	t_parent_task_id bigint,
	t_sub_task_est_hours decimal(10,2),
	t_sub_task_act_hours decimal(10,2),
	t_sequence int,
	t_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a new task to a project

	Params:		t_project_id ......... ID# of project to add this task to
				t_name ............... Name of task
				t_start_date ......... Task start date
				t_end_date ........... Task end date			
				t_define_done ........ Expected deliverable or outcome of task
				t_status_id .......... ID# of current status
				t_est_hours .......... Estimated hours required to complete task
				t_act_hours .......... Actual hours required to complete this task
				t_parent_task_id ..... If this is a sub-task, ID# of its parent
				t_sub_task_est_hours . Sum of estimated hours for all sub-tasks
				t_sub_task_act_hours . Sum of actual hours for all sub-tasks
				t_sequence ........... Ordinal position of this task when shown on boards
				t_user_id ............ ID# of person adding this task
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	INSERT INTO `mykanban`.`task`
		(`project_id`,
		`name`,
		`start_date`,
		`end_date`,
		`define_done`,
		`status_id`,
		`est_hours`,
		`act_hours`,
		`parent_task_id`,
		`sub_task_est_hours`,
		`sub_task_act_hours`,
		created_by,
		modified_by,
		created,
		modified,
		sequence)
	VALUES
		(t_project_id,
		t_name,
		t_start_date,
		t_end_date,
		t_define_done,
		t_status_id,
		t_est_hours,
		t_act_hours,
		t_parent_task_id,
		t_sub_task_est_hours,
		t_sub_task_act_hours,
		t_user_id,
		t_user_id,
		now(),
		now(),
		t_sequence
	);

	SELECT * FROM vw_task WHERE id = LAST_INSERT_ID();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_task_comment` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_add_task_comment`(
	t_task_id bigint, 
	t_comment varchar(2000),
	t_user_id bigint
	)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a comment to a task

	Params:		t_task_id ........ ID# of task to add this comment to
				t_comment ........ Text of comment to add
				t_user_id ........ ID# of person adding this comment
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	INSERT INTO task_comment (task_id, comment, created_by, modified_by, created, modified) 
	VALUES (t_task_id, t_comment, t_user_id, t_user_id, now(), now());

	-- Return the results
	SELECT * FROM task_comment WHERE id = LAST_INSERT_ID(); 

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_add_task_tag` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_add_task_tag`(
	t_task_id bigint, 
	t_tag varchar(100),
	t_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Add a new tag to a task

	Params:		t_task_id ...... ID# of task to add tag to
				t_tag .......... Text of tag
				t_user_id ...... ID# of person adding this tag
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	-- Determine whether the tag exists, if not add it,
	-- otherwise use the one that already exists
	DECLARE t_tag_id BIGINT;
	DECLARE t_task_tag_id BIGINT;

	SELECT id INTO t_tag_id FROM tag WHERE tag = t_tag;
	IF (t_tag_id IS NULL) THEN 
		INSERT INTO tag (tag, created_by, modified_by, created, modified) 
		VALUES (t_tag, t_user_id, t_user_id, now(), now());
		SET t_tag_id = LAST_INSERT_ID();
	END IF;

	-- Determine if the task tag already exists, if not, add it.
	SELECT id INTO t_task_tag_id FROM task_tag WHERE task_tag.task_id = t_task_id AND task_tag.tag_id = t_tag_id;
	IF (t_task_tag_id IS NULL) THEN
		INSERT INTO task_tag (task_id, tag_id, created_by, modified_by, created, modified) 
		VALUES (t_task_id, t_tag_id, t_user_id, t_user_id, now(), now());
	END IF;

	-- Return the results
	SELECT task_tag.*, tag.tag, tag.created, tag.created_by, tag.modified, tag.modified_by FROM task_tag, tag WHERE task_tag.task_id = t_task_id AND task_tag.tag_id = tag.id AND task_tag.tag_id = t_tag_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_all_data` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_delete_all_data`()
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Clear out the database

	Params:		NONE
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	delete from board_set where id > 0 limit 1000;
	delete from board where id > 0 limit 1000;
	delete from project where id > 0 limit 1000;
	delete from task where id > 0 limit 1000;
	delete from tag where id > 0 limit 1000;
	delete from comment where id > 0 limit 1000;
	delete from person where id > 0 limit 1000;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_approver_from_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_delete_approver_from_task`(
	t_task_id bigint, 
	t_person_id bigint)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove an approver from a task

	Params:		t_task_id ...... ID# of task to remove approver from
				t_person_id .... ID# of person to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	delete FROM task_approver 
		WHERE task_id = t_task_id 
		  AND person_id = t_person_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_assignee_from_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_delete_assignee_from_task`(
	t_task_id bigint, 
	t_person_id bigint)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove an assignee from a task

	Params:		t_task_id ...... ID# of task to remove assignee from
				t_person_id .... ID# of person to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	delete FROM task_assignee WHERE task_id = t_task_id AND person_id = t_person_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_board` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_delete_board`(
	b_board_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a board and all associated link rows

	Params:		b_board_id ..... ID# of board to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	DELETE FROM board WHERE id = b_board_id;
	DELETE FROM board_project WHERE board_id = b_board_id;
	DELETE FROM board_person WHERE board_id = b_board_id;
	DELETE FROM board_task WHERE board_id = b_board_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_board_set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_delete_board_set`(
	board_set_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a board from the system

	Params:		b_board_id ......... ID# of board to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	DELETE FROM board_set WHERE id = board_set_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_person` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_delete_person`(
	p_person_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a person from the system

	Params:		p_person_id .... ID# of person to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	DELETE FROM person WHERE id = p_person_id;
	DELETE FROM board_person WHERE person_id = p_person_id;
	DELETE FROM project_stakeholder WHERE person_id = p_person_id;
	DELETE FROM task_assignee WHERE person_id = p_person_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_project` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_delete_project`(
	p_project_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a project from the system

	Params:		p_project_id .... ID# of project to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	DELETE FROM project WHERE id = p_project_id;
	DELETE FROM board_project WHERE project_id = p_project_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_property` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_delete_property`(
	p_property_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a property from the database

	Params:		p_property_id ..... ID# of property to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	DELETE FROM property WHERE id = p_property_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_sprint` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_delete_sprint`(
	s_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a sprint from the system

	Params:		s_id ........ ID# of sprint to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	DELETE FROM sprint WHERE id = s_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_stakeholder_from_project` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_delete_stakeholder_from_project`(
	p_project_id bigint, 
	p_person_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a stakeholder (person) from a project

	Params:		p_project_id ..... ID# of project containing stakeholder
				p_person_id ...... ID# of person to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	delete FROM project_stakeholder 
		WHERE project_id = p_project_id 
		  AND person_id = p_person_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_status_code` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_delete_status_code`(
	status_code_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a status code from a board set

	Params:		status_code_id .... ID# of status code to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	DELETE FROM board_set_status WHERE id = status_code_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_tag` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_delete_tag`(
	t_tag_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a tag from the system

	Params:		t_tag_id ..... ID# of tag to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	DELETE FROM tag WHERE id = t_tag_id;
	DELETE FROM task_tag WHERE tag_id = t_tag_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_delete_task`(
	t_task_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a task from the system, along with all its links

	Params:		t_task_id ...... ID# of task to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	DELETE FROM task WHERE id = t_task_id;
	DELETE FROM task_assignee WHERE task_id = t_task_id;
	DELETE FROM task_comment where task_id = t_task_id;
	DELETE FROM task_person_preference WHERE task_id = t_task_id;
	DELETE FROM task_approver WHERE task_id = t_task_id;
	DELETE FROM task_tag WHERE task_id = t_task_id;
	DELETE FROM board_task WHERE task_id = t_task_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_task_comment` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_delete_task_comment`(
	t_task_comment_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a comment from a task

	Params:		t_task_comment_id .... ID# of comment to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	DELETE FROM task_comment WHERE id = t_task_comment_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_task_tag` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_delete_task_tag`(
	t_task_id bigint, 
	t_tag varchar(100)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a tag from a task

	Params:		t_task_id ...... ID# of task to remove comment from
				t_tag .......... Text of tag to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	DECLARE t_tag_id bigint;
	SELECT id INTO t_tag_id FROM tag WHERE tag = t_tag;
	DELETE FROM task_tag WHERE task_id = t_task_id AND tag_id = t_tag_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_delete_user_from_board` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_delete_user_from_board`(
	p_board_id bigint, 
	p_person_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Remove a person (user) from a Kanban board

	Params:		p_board_id .... ID# of Kanban board to remove user from
				p_person_id ... ID# of person to remove
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	delete FROM board_person 
		WHERE board_id = p_board_id 
		  AND person_id = p_person_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_approver` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_approver`(
	approver_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Return all data for a given approver

	Params:		approver_id ..... ID# of approver to return
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT DISTINCT person.*, task_approver.id approver_id, task_approver.task_id task_id
		FROM person, task_approver
		WHERE task_approver.id = approver_id
		  AND person.id = task_approver.person_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_approvers_by_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_approvers_by_task`(
	t_task_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Return all approvers for a given task

	Params:		t_task_id ...... ID# of task to return approvers for
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT p.*, ta.task_id, ta.person_id, ta.id approver_id
	
		FROM person p,
			task_approver ta

		WHERE ta.task_id = t_task_id 
		  AND p.id = ta.person_id

	ORDER BY name;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_assignees_by_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_assignees_by_task`(
	t_task_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Return all assignees for a given task

	Params:		t_task_id ...... ID# of task to return assignees for
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT p.*, ta.task_id, ta.person_id, ta.role 
	
		FROM person p,
			task_assignee ta

		WHERE ta.task_id = t_task_id 
		  AND p.id = ta.person_id

	ORDER BY name;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_board` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_board`(
	board_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Return all data for a given Kanban board

	Params:		board_id ..... ID# of board to return
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM board 
		WHERE id = board_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_boards_by_boardset_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_boards_by_boardset_id`(
	b_board_set_id bigint, 
	b_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Get all Kanban boards that are members of a board set

	Params:		b_board_set_id .... ID# of board set to find all boards for
				b_user_id ......... ID# of user (person) requesting boards
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM board, board_person 
		WHERE board_set_id = b_board_set_id 
		  AND board.id = board_person.board_id 
		  AND board_person.person_id = b_user_id
		  AND board_person.can_read = 1
	ORDER BY board.name;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_boards_by_name` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_boards_by_name`(
	name_filter varchar(255)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Get all boards with given text in their names

	Params:		name_filter ..... Text to search form
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM board 
		WHERE name LIKE CONCAT('%', name_filter, '%') 
	ORDER BY name;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_boards_by_user_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_boards_by_user_id`(
	b_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Find all boards that a given user (person) can access

	Params:		b_user_id .... ID# of user (person) whose boards we want to find
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM board, board_person 
		WHERE board.id = board_person.board_id 
		  AND board_person.person_id = b_user_id
		  AND board_person.can_read = 1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_board_set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_board_set`(
	board_set_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Get all data for a given board set

	Params:		board_set_id ..... ID# of board set to return
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM board_set 
		WHERE id = board_set_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_board_sets_by_name` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_board_sets_by_name`(
	name_filter varchar(255)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retriev all board sets with a given string in their name

	Params:		name_filter ..... Text to search for in board set name column
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM board_set 
		WHERE name LIKE CONCAT('%', name_filter, '%') 
	ORDER BY name;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_board_set_status_codes` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_board_set_status_codes`(
	b_board_set_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Get all status codes for a given board set

	Params:		b_board_set_id .... ID# of board set to retrieve status codes for
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM board_set_status 
		WHERE board_set_id = b_board_set_id 
	ORDER BY sequence;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_board_status_codes` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_board_status_codes`(
	board_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve status codes associated with a board's board set

	Params:		board_id ...... ID# of board to find status codes for
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM board_set_status 
		WHERE board_set_id = 
			(SELECT board_set_id FROM board WHERE id = board_id) 
	ORDER BY sequence;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_board_user` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_board_user`(
	user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Get a board user based on their unique ID#

	Params:		user_id .... ID# of Kanban user
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	select * FROM board_person 
		WHERE id = user_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_board_user_by_board_person` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_board_user_by_board_person`(
	u_board_id bigint,
	u_person_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Get a board user based on the board ID# and person ID#

	Params:		u_board_id .... ID# of Kanban board
				u_person_id ... ID# of person
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	select * FROM board_person 
		WHERE board_id = u_board_id
		  AND person_id = u_person_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_comments_by_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_comments_by_task`(
	t_task_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all comments for a given task

	Params:		t_task_id ...... ID# of task to to retrieve comments for
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM task_comment 
		WHERE task_id = t_task_id 
	ORDER BY comment;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_people_by_board` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_people_by_board`(
	b_board_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Get all users (persons) associated with a given board

	Params:		b_board_id ... ID# of board to retrieve users (persons) for
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT p.*, bp.board_id, bp.person_id, bp.can_add, bp.can_edit, bp.can_delete, bp.can_read 
	
		FROM person p,
			board_person bp

		WHERE bp.board_id = b_board_id AND p.id = bp.person_id

	ORDER BY name;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_people_by_name` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_people_by_name`(
	name_filter varchar(255)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all persons with names or user names matching the provided string

	Params:		name_filter ..... Text to match
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM person 
		WHERE name LIKE CONCAT('%', name_filter, '%') 
		   OR user_name LIKE CONCAT('%', name_filter, '%')
	ORDER BY name;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_people_by_project` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_people_by_project`(
	p_project_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all persons associated with a given project

	Params:		p_project_id .... ID# of project to retrieve persons for
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT p.*, ps.project_id, ps.person_id, ps.role 
	
		FROM person p,
			project_stakeholder ps

		WHERE ps.project_id = p_project_id AND p.id = ps.person_id

	ORDER BY name;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_person` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_person`(
	person_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all data for a specific individual

	Params:		person_id .... ID# of person to retrieve
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM person 
		WHERE id = person_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_project` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_project`(
	project_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all project data, and some board data, for a given project

	Params:		project_id ..... ID# of project to retrieve
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT 	p.*,
			bp.board_id board_id,
			b.name board_name,
			bs.name board_set_name,
			bs.id board_set_id

	FROM 	project p LEFT OUTER JOIN (board_project bp, board b, board_set bs)
				ON p.id = bp.project_id 
			AND b.id = bp.board_id 
			AND b.board_set_id = bs.id

	WHERE 	p.id = project_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_projects_by_board` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_projects_by_board`(b_board_id bigint)
BEGIN
	SELECT * 
	
	FROM project p,
		board_project bp

	WHERE bp.board_id = b_board_id AND p.id = bp.project_id

	ORDER BY name;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_projects_by_name` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_projects_by_name`(
	name_filter varchar(255)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all projects with names matching a search string

	Params:		name_filter ..... Text to search for
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM project 
		WHERE name LIKE CONCAT('%', name_filter, '%');
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_properties_by_object` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_properties_by_object`(
	p_parent_type varchar(100),
	p_parent_id bigint,
	p_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retriev all properties for a given object

	Params:		p_parent_type .... table of row owning properties (e.g. task, project, etc.)
				p_parent_id ...... ID# of object owning properties
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM property 
		WHERE parent_type = p_parent_type 
		  AND parent_id = p_parent_id
	ORDER BY name;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_property` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_property`(
	p_property_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Return a given property

	Params:		p_property_id ..... ID# of property to return
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM property 
		WHERE id = p_property_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_sprint` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_sprint`(
	sprint_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve a sprint by its id

	Params:		sprint_id .... ID# of sprint to retreive
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM sprint 
		WHERE id = sprint_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_sprints_by_board` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_sprints_by_board`(
	b_board_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all sprints for a given Kanban board

	Params:		b_board_id .... ID# of Kanban board to retrieve sprints for
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM sprint 
		WHERE board_id = b_board_id 
	ORDER BY start_date, sequence;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_status_code` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_status_code`(
	status_code_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Get a specific status code entry

	Params:		status_code_id ... ID# of status code to retrieve
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM board_set_status 
		WHERE id = status_code_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_status_codes_by_board_set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_status_codes_by_board_set`(
	b_board_set_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all status codes for a given board set

	Params:		b_board_set_id ... ID# of board set to retrieve status codes for
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM board_set_status 
		WHERE board_set_id = b_board_set_id 
	ORDER BY sequence;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_status_codes_by_name` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_status_codes_by_name`(
	name_filter varchar(255)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all status codes matching the provided search text

	Params:		name_filter .... Text to match
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM board_status 
		WHERE name LIKE CONCAT('%', name_filter, '%');
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_subtasks_for_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_subtasks_for_task`(
	t_parent_task_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all sub-tasks of a given task

	Params:		t_parent_task_id .... ID# of parent task
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM task 
		WHERE parent_task_id = t_parent_task_id 
	ORDER BY name;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_tag` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_tag`(
	t_tag_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve a specific tag by its id

	Params:		t_tag_id ..... ID# of tag to retrieve
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM tag 
		WHERE id = t_tag_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_tags_by_name` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_tags_by_name`(
	name_filter varchar(255)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all tags matching a specific search string

	Params:		name_filter .... Text to match
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT task_tag.*, tag.tag, tag.created, tag.created_by, tag.modified, tag.modified_by 
		FROM task_tag, tag 
		WHERE tag LIKE CONCAT('%', name_filter, '%') 
		  AND task_tag.tag_id = tag.id
		ORDER BY tag;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_tags_by_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_tags_by_task`(
	t_task_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all tags for a given task

	Params:		t_task_id ...... ID# of task to find tags for
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT task_tag.*, tag.tag, tag.created, tag.created_by, tag.modified, tag.modified_by 
		FROM task_tag, tag 
		WHERE task_tag.task_id = t_task_id 
		  AND task_tag.tag_id = tag.id
		ORDER BY tag.tag;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_tags_for_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_tags_for_task`(t_task_id bigint)
BEGIN
	SELECT * FROM task_tag, tag WHERE task_tag.task_id = t_task_id AND task_tag.tag_id = tag.id ORDER BY tag.tag;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_task`(
	task_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve a task and some associated project data by its id

	Params:		t_task_id ...... ID# of task to retrieve
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT 	DISTINCT t.*, 
			p.name project_name,
			bp.board_id board_id,
			b.name board_name,
			bs.name board_set_name,
			bs.id board_set_id

	FROM 	task t LEFT OUTER JOIN (project p, board_project bp, board b, board_set bs)
			ON t.project_id = p.id 
			AND p.id = bp.project_id 
			AND b.id = bp.board_id 
			AND b.board_set_id = bs.id

	WHERE 	t.id = task_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_tasks_by_board` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_tasks_by_board`(
	t_board_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retriev all top-level tasks for a given board

	Params:		t_board_id .... ID# of board these tasks belong to
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT task.*, project.name project_name FROM task, project
		WHERE project_id IN (SELECT project_id FROM board_project WHERE board_id = t_board_id)
		  AND task.project_id = project.id
		  AND parent_task_id = 0 
	ORDER BY sequence;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_tasks_by_name` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_tasks_by_name`(
	name_filter varchar(255)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all tasks matching a given search string

	Params:		name_filter .... Text to match
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM task 
		WHERE name LIKE CONCAT('%', name_filter, '%');
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_tasks_by_project` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_tasks_by_project`(
	p_project_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retriev all top-level tasks for a given project

	Params:		p_project_id .... ID# of project whose tasks we want to retrieve
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM task 
		WHERE project_id = p_project_id 
		  AND parent_task_id = 0 
	ORDER BY name;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_task_comment` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_task_comment`(
	t_task_comment_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve a task comment by its id

	Params:		t_task_comment_id ... ID# of comment to retrieve
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM task_comment 
		WHERE id = t_task_comment_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_task_comments_by_name` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_get_task_comments_by_name`(
	name_filter varchar(255)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all task comments matching a given search string

	Params:		name_filter ... Text to match
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM task_comment 
		WHERE comment LIKE CONCAT('%', name_filter, '%');
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_get_task_tag` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_get_task_tag`(
	t_task_tag_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Retrieve all data for a given task tag

	Params:		t_task_tag_id .... ID# of task tag to return
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT task_tag.*, tag.tag, tag.created, tag.created_by, tag.modified, tag.modified_by 
		FROM task_tag, tag 
		WHERE task_tag.id = t_task_tag_id 
		  AND task_tag.tag_id = tag.id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_login` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_login`(
	p_user_name varchar(100), 
	p_password varchar(100)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Return user (person) data where user name + password matches

	Params:		p_user_name .... User name to match
				p_password ..... Encrypted password to match
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	SELECT * FROM person 
		WHERE user_name = p_user_name 
		  AND password = p_password;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_select_all_data` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_select_all_data`()
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Select all data from the core tables

	Params:		NONE
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	select * from board_set;
	select * from board;
	select * from project;
	select * from task;
	select * from person;
	select * from comment;
	select * from tag;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_update_board` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_update_board`(
	board_id bigint, 
	board_name varchar(100),
	b_board_set_id bigint,
	b_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Update board data given its id

	Params:		board_id ........ ID# of board to update
				board_name ...... Name of board
				b_board_set_id .. Board set this board belongs to
				b_user_id ....... ID# of user (person) updating this board
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	UPDATE board 

		SET name = board_name, 
			board_set_id = b_board_set_id,
			modified_by = b_user_id,
			modified = now()

	WHERE id = board_id;

	SELECT * FROM board WHERE id = board_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_update_board_set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_update_board_set`(
	board_set_id bigint, 
	board_set_name varchar(100),
	bs_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Update a board set givent its id

	Params:		board_set_id .... ID# of board set to update
				board_set_name .. Name of board set
				bs_user_id ...... ID# of user (person) updating this board
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	UPDATE board_set

		SET name = board_set_name, 
		modified_by = bs_user_id, 
		modified = now()

	WHERE id = board_set_id;

	SELECT * FROM board_set WHERE id = board_set_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_update_comment` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_update_comment`(
	t_task_id bigint, 
	t_comment_id bigint, 
	t_comment varchar(2000),
	t_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Update a task comment, given its id

	Params:		t_task_id ..... ID# of task this comment belongs to
				t_comment_id .. ID# of comment
				t_comment ..... Text of comment
				t_user_id ..... ID# of user (person) updating this comment
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	UPDATE task_comment 

		SET comment = t_comment, 
			task_id = t_task_id,
			modified_by = t_user_id,
			modified = now()

	WHERE id = t_comment_id;

	SELECT * FROM task_comment WHERE id = t_comment_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_update_person` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_update_person`(
	person_id bigint,
	person_name varchar(100),
	person_picture_url varchar(255),
	person_email varchar(100),
	person_phone varchar(100),
	person_user_name varchar(100),
	person_password varchar(100),
	person_can_login tinyint,
	p_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Update a person entry given its id

	Params:		person_id ............. ID# of person entry to update
				person_name ........... First & last name of person
				person_picture_url .... Url of picture
				person_email .......... Email address
				person_phone .......... Phone #
				person_user_name ...... User login name
				person_password ....... Encrypted password
				person_can_login ...... 1=this user can login to the system
				p_user_id ............. ID# of person (user) updating this entry
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	UPDATE `mykanban`.`person`
	SET
		`name` = person_name,
		`picture_url` = person_picture_url,
		`email` = person_email,
		`phone` = person_phone,
		`user_name` = person_user_name,
		`password` = person_password,
		`can_login` = person_can_login,
		modified_by = p_user_id,
		modified = now()

	WHERE `id` = person_id;

	SELECT * FROM person WHERE id = person_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_update_project` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_update_project`(
		project_id bigint,
		p_name varchar(100),
		p_project_lead bigint,
		p_expected_start_date date,
		p_expected_end_date date,
		p_earliest_task_start_date date,
		p_latest_task_end_date date,
		p_status bigint,
		p_define_done varchar(1000),
		p_task_est_hours decimal(10,2),
		p_task_act_hours decimal(10,2),
		p_user_id bigint)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Update a project entry given its id

	Params:		project_id ................ ID# of project to update
				p_name .................... Name of project
				p_project_lead ............ ID# of person who is project lead
				p_expected_start_date ..... Date project expected to start
				p_expected_end_date ....... Date project expected to end
				p_earliest_task_start_date  Earliest date for all tasks for this project
				p_latest_task_end_date .... Latest date for all tasks for this project
				p_status .................. ID# of current status of this project
				p_define_done ............. Expected outcome or result when complete
				p_task_est_hours .......... Sum of all task estimated hours
				p_task_act_hours .......... Sum of all task actual hours
				p_user_id ................. ID# of user (person) making the update
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	UPDATE `mykanban`.`project`

	SET
		`name` = p_name,
		`project_lead` = p_project_lead,
		`expected_start_date` = p_expected_start_date,
		`expected_end_date` = p_expected_end_date,
		`earliest_task_start_date` = p_earliest_task_start_date,
		`latest_task_end_date` = p_latest_task_end_date,
		`status` = p_status,
		`define_done` = p_define_done,
		`task_est_hours` = p_task_est_hours,
		`task_act_hours` = p_task_act_hours,
		modified_by = p_user_id,
		modified = now()

	WHERE `id` = project_id;

	SELECT * FROM project WHERE id = project_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_update_property` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_update_property`(
	p_property_id bigint,
	p_name varchar(100),
	p_value varchar(1000),
	p_value_type varchar(100),
	p_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Update board data given its id

	Params:		p_property_id ........ ID# of property to update
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	UPDATE property 

		SET name = p_name,
			value = p_value,
			value_type = p_value_type,
			modified_by = p_user_id,
			modified = now()

	WHERE id = p_property_id;

	SELECT * FROM property WHERE id = p_property_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_update_sprint` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_update_sprint`(
	s_id bigint,
	s_board_id bigint,
	s_start_date datetime,
	s_end_date datetime,
	s_sequence int,
	s_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Update a sprint given its id

	Params:		s_id ............... ID# of sprint to update
				s_board_id ......... ID# of board this sprint belongs to
				s_start_date ....... Begin date for this sprint
				s_end_date ......... End date for this sprint
				s_sequence ......... Ordinal position of this sprint
				s_user_id .......... ID# of user (person) making this update
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	UPDATE sprint

		SET `board_id` = s_board_id,
		`start_date` = s_start_date,
		`end_date` = s_end_date,
		`sequence` = s_sequence,
		modified_by = s_user_id,
		modified = now()

	WHERE id = s_id;

	SELECT * FROM sprint WHERE id = s_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_update_status_code` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_update_status_code`(
	s_id bigint,
	s_board_set_id bigint,
	s_column_heading varchar(20),
	s_status varchar(100),
	s_sequence int,
	s_user_id bigint,
	s_fore_color varchar(45),
	s_back_color varchar(45)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Update a status code given its id

	Params:		s_id .............. ID# of status code entry to update
				s_board_set_id .... ID# of board set this status applies to
				s_column_heading .. Column heading to display on Kanban board
				s_status .......... Internal name for this status
				s_sequence ........ Ordinal position of this status code
				s_user_id ......... ID# of user (person) making this update
				s_fore_color ...... Foreground color to use to display tasks with this status
				s_back_color ...... Background color to use to display tasks with this status
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	UPDATE board_set_status

		SET `board_set_id` = s_board_set_id,
		`column_heading` = s_column_heading,
		`status` = s_status,
		`sequence` = s_sequence,
		modified_by = s_user_id,
		modified = now(),
		fore_color = s_fore_color,
		back_color = s_back_color

	WHERE id = s_id;

	SELECT * FROM board_set_status WHERE id = s_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_update_tag` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_update_tag`(
	t_tag_id bigint, 
	t_tag varchar(100)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Update a tag given its id

	Params:		t_tag_id ...... ID# of the tag to update
				t_tag ......... New text of tag
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	UPDATE tag SET tag = t_tag 
		WHERE id = t_tag_id;

	SELECT * FROM tag WHERE id = t_tag_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_update_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_update_task`(
	t_task_id bigint,
	t_project_id bigint,
	t_name varchar(100),
	t_start_date date,
	t_end_date date,
	t_define_done varchar(1000),
	t_status_id bigint,
	t_est_hours decimal(10,2),
	t_act_hours decimal(10,2),
	t_parent_task_id bigint,
	t_sub_task_est_hours decimal(10,2),
	t_sub_task_act_hours decimal(10,2),
	t_sequence int,
	t_user_id bigint
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Update task

	Params:		t_task_id .............. ID# of task to update
				t_project_id ........... ID# of project this task belongs to
				t_name ................. Name of task
				t_start_date ........... Start date of task
				t_end_date ............. End date of task
				t_define_done .......... Expected deliverable or outcome of task
				t_status_id ............ ID# of current task status
				t_est_hours ............ Estimated hours to complete this task
				t_act_hours ............ Actual hours to complete this task
				t_parent_task_id ....... For sub-tasks, the ID# of its parent task
				t_sub_task_est_hours ... Sum of estimate hours for all sub-tasks
				t_sub_task_act_hours ... Sum of actual hours for all sub-tasks
				t_sequence ............. Ordinal position of this task when shown on boards
				t_user_id .............. ID# of user (person) making this update
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	-- Task can only be assigned to a project once, when its
	-- project id value is 0, otherwise keep the same project
	-- id

	SET @pid = t_project_id;
/*
	SET @pid = (SELECT project_id FROM task WHERE id = t_task_id);
	IF (t_project_id <> 0 AND @pid = 0) THEN 
		SET @pid = t_project_id;
	END IF;
*/

	UPDATE `mykanban`.`task`
	SET
		`id` = t_task_id,
		`project_id` = @pid,
		`name` = t_name,
		`start_date` = t_start_date,
		`end_date` = t_end_date,
		`define_done` = t_define_done,
		`status_id` = t_status_id,
		`est_hours` = t_est_hours,
		`act_hours` = t_act_hours,
		`parent_task_id` = t_parent_task_id,
		`sub_task_est_hours` = t_sub_task_est_hours,
		`sub_task_act_hours` = t_sub_task_act_hours,
		modified_by = t_user_id,
		modified = now(),
		sequence = t_sequence

	WHERE `id` = t_task_id;

	SELECT * FROM vw_task WHERE id = t_task_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_update_task_tag` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `sp_update_task_tag`(
	t_task_id bigint, 
	t_tag varchar(100)
)
BEGIN
/* 	
	---------------------------------------------------------------------------
	Purpose:	Update task

	Params:		t_task_id ..... ID# of task containing tag to update
				t_tag ......... Text of tag to update
	---------------------------------------------------------------------------
	Mods:
	---------------------------------------------------------------------------
*/
	-- Get the ID# of tag based on its text
	DECLARE t_tag_id BIGINT;
	SELECT id INTO t_tag_id FROM tag WHERE tag = t_tag;

	-- If tag not already in database, add it
	IF (t_tag_id IS NULL) THEN 
		INSERT INTO tag (tag) VALUES (t_tag);
		SET t_tag_id = LAST_INSERT_ID();
	END IF;

	UPDATE tag SET tag = t_tag 
		WHERE id = t_tag_id;

	SELECT * FROM task_tag, tag 
		WHERE task_tag.id = t_task_id 
		  AND task_tag.tag_id = tag.id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `_dont_use_sp_get_tags_for_task` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`mgerow`@`%` PROCEDURE `_dont_use_sp_get_tags_for_task`(t_task_id bigint)
BEGIN
	SELECT * FROM task_tag, tag WHERE task_tag.task_id = t_task_id AND task_tag.tag_id = tag.id ORDER BY tag.tag;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `vw_board`
--

/*!50001 DROP TABLE IF EXISTS `vw_board`*/;
/*!50001 DROP VIEW IF EXISTS `vw_board`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`mgerow`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_board` AS select `board`.`id` AS `id`,`board`.`name` AS `name`,`board`.`board_set_id` AS `board_set_id` from `board` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `vw_person`
--

/*!50001 DROP TABLE IF EXISTS `vw_person`*/;
/*!50001 DROP VIEW IF EXISTS `vw_person`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`mgerow`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_person` AS select `person`.`id` AS `id`,`person`.`name` AS `name`,`person`.`picture_url` AS `picture_url`,`person`.`email` AS `email`,`person`.`phone` AS `phone`,`person`.`user_name` AS `user_name`,`person`.`password` AS `password`,`person`.`can_login` AS `can_login` from `person` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `vw_project`
--

/*!50001 DROP TABLE IF EXISTS `vw_project`*/;
/*!50001 DROP VIEW IF EXISTS `vw_project`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`mgerow`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_project` AS select `project`.`id` AS `id`,`project`.`name` AS `name`,`project`.`project_lead` AS `project_lead`,`project`.`expected_start_date` AS `expected_start_date`,`project`.`expected_end_date` AS `expected_end_date`,`project`.`earliest_task_start_date` AS `earliest_task_start_date`,`project`.`latest_task_end_date` AS `latest_task_end_date`,`project`.`status` AS `status`,`project`.`define_done` AS `define_done`,`project`.`task_est_hours` AS `task_est_hours`,`project`.`task_act_hours` AS `task_act_hours` from `project` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `vw_tag`
--

/*!50001 DROP TABLE IF EXISTS `vw_tag`*/;
/*!50001 DROP VIEW IF EXISTS `vw_tag`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`mgerow`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_tag` AS select `tag`.`id` AS `id`,`tag`.`tag` AS `tag` from `tag` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `vw_task`
--

/*!50001 DROP TABLE IF EXISTS `vw_task`*/;
/*!50001 DROP VIEW IF EXISTS `vw_task`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`mgerow`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_task` AS select `task`.`id` AS `id`,`task`.`project_id` AS `project_id`,`task`.`name` AS `name`,`task`.`start_date` AS `start_date`,`task`.`end_date` AS `end_date`,`task`.`define_done` AS `define_done`,`task`.`status_id` AS `status_id`,`task`.`est_hours` AS `est_hours`,`task`.`act_hours` AS `act_hours`,`task`.`parent_task_id` AS `parent_task_id`,`task`.`sub_task_est_hours` AS `sub_task_est_hours`,`task`.`sub_task_act_hours` AS `sub_task_act_hours` from `task` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-02-21 12:07:06
