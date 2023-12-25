
# MovieMinimalAPI_ES

Ce projet est le quatrième brief de ma formation en .NET . Le but est de récupérer la base de données d'un camarade de promotion (répuperer son environnement de développement) et de l'exploiter au travers de plusieurs requêtes rédigées en language C#.

## Installation 

Pour ce projet, j'ai utilisé l'ensemble de logiciels XAMPP téléchargeable ici : 

https://www.apachefriends.org/fr/index.html

Une fois téléchargé, il faut commencer par se rendre sur "Manager" et lancer les trois serveurs dans "Manage server". Une fois les serveurs lancés, il faut se rendre dans "Welcome", cliquer sur "Go to application" et aller dans l'onglet "phpMyAdmin". Une fois sur cet onglet, vous pouvez vous rendre dans SQL et rentrer dans le terminal : 

```bash
  CREATE DATABASE IF NOT EXISTS streaming; 
  USE streaming;
```  
Une fois la base de données crée, vous aller pouvoir l'alimenter et copier/coller l'intégralité du code ci-dessous dans le terminal SQL de "streaming".

```bash
CREATE TABLE User`(
   Id_user INT AUTO_INCREMENT,
   email VARCHAR(100),
   password VARCHAR(50),
   creation_date_user DATE,
   modification_date_user DATE,
   PRIMARY KEY(Id_user)
);

CREATE TABLE Actor(
   Id_actor INT AUTO_INCREMENT,
   firstname_actor VARCHAR(50),
   lastname_actor VARCHAR(50),
   birthdate_actor DATE,
   creation_date_actor DATE,
   modification_date_actor DATE,
   PRIMARY KEY(Id_actor)
);

CREATE TABLE Director(
   Id_director INT AUTO_INCREMENT,
   fistname_director VARCHAR(50),
   lastname_director VARCHAR(50),
   creation_date_director DATE,
   modification_date_director DATE,
   PRIMARY KEY(Id_director)
);

CREATE TABLE Movie(
   Id_movie INT AUTO_INCREMENT,
   title VARCHAR(50),
   duration INT,
   release_year DATE,
   creation_date_movie DATE,
   modification_date_movie DATE,
   Id_director INT,
   PRIMARY KEY(Id_movie),
   FOREIGN KEY(Id_director) REFERENCES Director(Id_director)
);

CREATE TABLE Favorite(
   Id_user INT,
   Id_movie INT,
   PRIMARY KEY(Id_user, Id_movie),
   FOREIGN KEY(Id_user) REFERENCES Userr(Id_user),
   FOREIGN KEY(Id_movie) REFERENCES Movie(Id_movie)
);

CREATE TABLE Perform(
   Id_movie INT,
   Id_actor INT,
   role VARCHAR(50),
   is_lead_role BOOLEAN,
   PRIMARY KEY(Id_movie, Id_actor),
   FOREIGN KEY(Id_movie) REFERENCES Movie(Id_movie),
   FOREIGN KEY(Id_actor) REFERENCES Actor(Id_actor)
);

INSERT INTO Userr (email, password, creation_date_user, modification_date_user)
VALUES 
  ('user1@example.com', 'password1', CURRENT_DATE, '2023-03-01'),
  ('user2@example.com', 'password2', CURRENT_DATE, '2023-03-02'),
  ('user3@example.com', 'password3', CURRENT_DATE, '2023-03-03'),
  ('user4@example.com', 'password4', CURRENT_DATE, '2023-03-04'),
  ('user5@example.com', 'password5', CURRENT_DATE, '2023-03-05');

INSERT INTO Actor (firstname_actor, lastname_actor, birthdate_actor, creation_date_actor, modification_date_actor)
VALUES
  ('Marlon', 'Brando', '1924-04-03', CURRENT_DATE, '2023-03-01'),
  ('Robert', 'De Niro', '1943-08-17', CURRENT_DATE, '2023-03-02'),
  ('Al', 'Pacino', '1940-04-25', CURRENT_DATE, '2023-03-03'),
  ('Keanu', 'Reeves', '1964-09-02', CURRENT_DATE, '2023-03-04');

INSERT INTO Director (fistname_director, lastname_director, creation_date_director, modification_date_director)
VALUES
  ('Francis', 'Ford Coppola', CURRENT_DATE, '2023-03-01'),
  ('Martin', 'Scorsese', CURRENT_DATE, '2023-03-02'),
  ('Brian', 'De Palma', CURRENT_DATE, '2023-03-03'),
  ('Lana', 'Wachowski', CURRENT_DATE, '2023-03-04'),
  ('Michael', 'Mann', CURRENT_DATE, '2023-03-05');

INSERT INTO Movie (title, duration, release_year, creation_date_movie, modification_date_movie, Id_director)
VALUES
  ('Le Parrain', 175, '1972-03-24', CURRENT_DATE, '2023-03-01', 1),
  ('Les Affranchis', 146, '1990-09-09', CURRENT_DATE, '2023-03-02', 2),
  ('Scarface', 170, '1983-12-09', CURRENT_DATE, '2023-03-03', 3),
  ('Matrix', 136, '1999-03-31', CURRENT_DATE, '2023-03-04', 4),
  ('Heat', 170, '1995-12-15', CURRENT_DATE, '2023-03-05', 5);

INSERT INTO Favorite (Id_user, Id_movie)
VALUES
  (1, 1),
  (2, 2),
  (3, 3),
  (4, 4),
  (5, 5);

INSERT INTO Perform (Id_movie, Id_actor, role, is_lead_role)
VALUES
  (1, 1, 'Vito Corleone', TRUE),
  (2, 2, 'Jimmy Conway', TRUE),
  (3, 3, 'Tony Montana', TRUE),
  (4, 4, 'Neo', TRUE),
  (5, 3, 'Lt. Vincent Hanna', TRUE);
  (5, 2, 'Neil McCauley', TRUE),
```  

Lorsque vous avez injecté le SQL dans la base de données, vous pouvez quitter "phpMyAdmin" et l'applciation pour revenir sur "Manage server", vérifiez que les serveurs tourne toujours bien. Allez sur "MySQL Database", cliquez sur "configure" pour vérifier le port local sur lequel tourne la base de données. 

RDV maintenant sur VS ou VS Code sur le projet et assurez vous qu'aux lignes 29/60 et 87 de program.cs, MySQLConnection tourne bien sur le port local de votre database. Une fois cela fait, vous pouvez aller dans le terminal de votre éditeur de code et saisir : 

```bash 
dotnet run
```
Vous allez être redirigé vers Swagger dans une fenêtre de votre navigateur. Si jamais rien de ne s'affiche, retourner dans votre éditeur de code pour vérifier dans votre sorie de build que la fenêtre de Swagger écoute le bon port de sortie. N'hésitez pas à changer le port dans votre navigateur au besoin !

## Fonctionnalités

Et voilà ! Vous pouvez désormais intéragir avec la database "streaming" !

À partir d'ici, vous allez pouvoir :

- Obtenir les titre et date de sortie des films, du plus récent au plus ancien
- Obtenir les titre et date de sortie d'un film en particulier 
- Ajouter un film 
- Ajouter un acteur/actrice 
- Modifier un film 
- Supprimer un acteur/actrice


## Auteurs

- [@EnguerranSGG](https://github.com/EnguerranSGG)
