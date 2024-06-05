
CREATE DATABASE `mydatabase` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;



CREATE TABLE `users` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) DEFAULT NULL,
  `Email` varchar(200) DEFAULT NULL,
  `Password` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci



DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddUser`(
_id int,
_name varchar(200),
_email varchar(200),
_password varchar(200)
)
BEGIN
insert into users(name, email, password) values (_name, _email, _password);
END$$
DELIMITER ;




DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteUser`(
_id int
)
BEGIN
delete from users 
where id = _id;
END$$
DELIMITER ;



DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUser`(
_id int
)
BEGIN
select * 
from users 
where id = _id;
END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUsers`(
)
BEGIN
 SELECT * 
 FROM users;
END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdateUser`(
 _id int,
 _name varchar(200),
 _email varchar(200),
 _password varchar(200)
)
BEGIN
  if _id > 0 then
   update users 
   set 
   name = _name,
   email = _email,
   password = _password
   where id = _id;
  end if;
END$$
DELIMITER ;


