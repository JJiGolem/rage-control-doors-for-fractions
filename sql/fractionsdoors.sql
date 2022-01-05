-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1
-- Время создания: Сен 09 2021 г., 12:52
-- Версия сервера: 10.4.20-MariaDB
-- Версия PHP: 8.0.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `golemo`
--

-- --------------------------------------------------------

--
-- Структура таблицы `fractionsdoors`
--

CREATE TABLE `fractionsdoors` (
  `id` int(11) NOT NULL,
  `fractionId` int(11) NOT NULL,
  `fractionRank` int(11) NOT NULL,
  `RightPosition` varchar(255) DEFAULT NULL,
  `LeftPosition` varchar(255) DEFAULT NULL,
  `Model` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `fractionsdoors`
--

INSERT INTO `fractionsdoors` (`id`, `fractionId`, `fractionRank`, `RightPosition`, `LeftPosition`, `Model`) VALUES
(0, 7, 1, '{\"x\":469.9679, \"y\":-1014.452, \"z\":26.53623}', '{\"x\":467.3716, \"y\":-1014.452, \"z\":26.53623}', '-2023754432'),
(2, 7, 7, '{\"x\":443.4078, \"y\":-989.4454, \"z\":30.8393}', '{\"x\":446.0079, \"y\":-989.4454, \"z\":30.8393}', '185711165'),
(3, 7, 1, '{\"x\":443.0298, \"y\":-991.941, \"z\":30.8393}', '{\"x\":443.0298, \"y\":-994.5412, \"z\":30.8393}', '-131296141'),
(4, 7, 1, '{\"x\":450.1041, \"y\":-984.0915, \"z\":30.8393}', '{\"x\":450.1041, \"y\":-981.4915, \"z\":30.8393}', '185711165'),
(5, 7, 1, '{\"x\":446.5728, \"y\":-980.0106, \"z\":30.8393}', NULL, '-1320876379'),
(6, 7, 1, '{\"x\":450.1041, \"y\":-985.7384, \"z\":30.8393}', NULL, '1557126584'),
(7, 7, 1, '{\"x\":452.6248, \"y\":-987.3626, \"z\":30.8393}', NULL, '-2023754432');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `fractionsdoors`
--
ALTER TABLE `fractionsdoors`
  ADD PRIMARY KEY (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
