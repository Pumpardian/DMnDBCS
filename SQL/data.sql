-- 1. Заполнение таблицы users
INSERT INTO users (name, email, password) VALUES
('Alex Dante', 'alex.dante@example.com', 'password1'),
('Josh Bush', 'josh.bush@example.com', 'password2'),
('Lycia Arran', 'lycia.arran@example.com', 'password3'),
('Helena Read', 'helena.read@example.com', 'password4'),
('Dmitry Popov', 'dmitry.popov@example.com', 'password5'),
('Olga Nezhna', 'olga.nezhna@example.com', 'password6'),
('Frank Kerren', 'frank.kerren@example.com', 'password7'),
('Joe Majisk', 'joe.majisk@example.com', 'password8'),
('Nicolas Fredon', 'nicolas.fredon@example.com', 'password9'),
('Kate Marew', 'kate.marew@example.com', 'password10');

-- 2. Заполнение таблицы roles
INSERT INTO roles (name) VALUES
('Project Manager'),
('Developer'),
('Tester'),
('Designer'),
('Analyst');

-- 3. Заполнение таблицы userprofiles
INSERT INTO userprofiles (user_id, phone, address, date_of_birth, profile_picture) VALUES
(1, '+375291234567', 'Pobedy st., 2', '1990-01-01', 'path/to/profile1.jpg'),
(2, '+375291234568', 'Masherova st., 1', '1991-02-02', NULL),
(3, '+375291234569', 'Mira st., 3', '1992-03-03', 'path/to/profile3.jpg'),
(4, '+375291234570', 'Sovietskaya st., 4', '1993-04-04', 'path/to/profile4.jpg'),
(5, '+375291234571', 'Kirova st., 5', '1994-05-05', NULL),
(6, '+375291234572', 'Pushkina st., 6', '1995-06-06', 'path/to/profile6.jpg'),
(7, '+375291234573', 'Gagarin st., 7', '1996-07-07', 'path/to/profile7.jpg'),
(8, '+375291234574', 'Voroshilova st., 8', '1997-08-08', 'path/to/profile8.jpg'),
(9, '+375291234575', 'Paskevicha st., 9', '1998-09-09', NULL),
(10, '+375291234576', 'Orlovskaya st., 10', '1999-10-10', 'path/to/profile10.jpg');

-- 4. Заполнение таблицы taskstatuses
INSERT INTO taskstatuses (name) VALUES
('Planned'),
('Canceled'),
('In progress'),
('Completed');

-- 5. Заполнение таблицы projects
INSERT INTO projects (title, description, start_date, end_date) VALUES
('Project A', 'Project A description', '2025-01-01', '2025-06-01'),
('Project B', 'Project B description', '2025-02-15', NULL),
('Project C', NULL, '2025-04-10', '2025-09-10'),
('Project D', 'Project D description', '2024-04-20', '2024-12-31'),
('Project E', 'Project E description', '2025-05-05', NULL);

-- 6. Заполнение таблицы tasks
INSERT INTO tasks (title, description, status, creation_date, completion_date, project_id, executor_id) VALUES
('Task 1', 'Task 1 description', 1, '2025-06-01 10:00:00', NULL, 1, 2),
('Task 2', 'Task 2 description', 3, '2025-06-02 11:00:00', NULL, 1, 3),
('Task 3', 'Task 3 description', 2, '2025-06-03 12:00:00', '2025-06-05 15:00:00', 2, 5),
('Task 4', 'Task 4 description', 2, '2025-06-04 13:00:00', NULL, 2, 6),
('Task 5', 'Task 5 description', 3, '2025-06-05 14:00:00', NULL, 3, 8),
('Task 6', NULL, 1, '2025-06-06 15:00:00', NULL, 3, 8),
('Task 7', 'Task 7 description', 4, '2024-06-07 16:00:00', '2024-06-08 17:00:00', 4, 9),
('Task 8', 'Task 8 description', 1, '2025-06-08 17:00:00', NULL, 5, 10),
('Task 9', 'Task 9 description', 3, '2025-06-09 18:00:00', NULL, 5, 10),
('Task 10', 'Task 10 description', 4, '2025-06-10 19:00:00', '2025-06-11 20:00:00', 1, 2),
('Task 11', 'Task 11 description', 3, '2025-05-01 08:00:00', '2025-06-01 08:00:00', 1, 2),
('Task 12', 'Task 12 description', 3, '2025-05-02 09:00:00', '2025-06-02 09:00:00', 1, 3),
('Task 13', 'Task 13 description', 3, '2025-05-03 10:00:00', '2025-06-03 10:00:00', 1, 2),
('Task 14', 'Task 14 description', 3, '2025-05-04 11:00:00', '2025-06-04 11:00:00', 1, 3);


-- 7. Заполнение таблицы taskcomments
INSERT INTO taskcomments (content, creation_date, task_id, author_id) VALUES
('Task 1 commentary', '2025-06-01 11:00:00', 1, 2),
('Task 1 second commentary', '2025-06-01 12:00:00', 1, 3),
('Task 2 commentary', '2025-06-02 13:00:00', 2, 3),
('Task 3 review', '2025-06-03 14:00:00', 3, 5),
('Task 4 note', '2024-06-04 15:00:00', 4, 6),
('Task 5 discussion', '2025-06-05 16:00:00', 5, 8),
('Task 6 update', '2025-06-06 17:00:00', 6, 8),
('Task 7 note', '2025-06-07 18:00:00', 7, 9),
('Thoughts about Task 8', '2025-06-08 19:00:00', 8, 10),
('Ideas for Task 9', '2025-06-09 20:00:00', 9, 10);

-- 8. Заполнение таблицы userroles
INSERT INTO userroles (user_id, project_id, role_id) VALUES
(1, 1, 1),
(2, 1, 2),
(3, 1, 3),
(4, 2, 1),
(5, 2, 2),
(6, 2, 4),
(7, 3, 1),
(8, 3, 2),
(9, 4, 5),
(10, 5, 4);

-- 9. Заполнение таблицы projectresources
INSERT INTO projectresources (description, type, project_id) VALUES
('Resource 1 for Project A', 'Facility', 1),
('Resource 2 for Project A', 'Software', 1),
('Resource 1 for Project B', 'Software', 2),
(NULL, 'Documentation', 3),
('Resource 2 for Project C', 'Facility', 3),
('Resource 1 for Project D', 'Software', 4),
('Resource 1 for Project E', 'Facility', 5);

-- 10. Заполнение таблицы logs
INSERT INTO logs (action, date, user_id) VALUES
('User logged in', '2025-06-01 09:00:00', 1),
('User logged out', '2025-06-01 17:00:00', 1),
('User updated a profile', '2025-06-02 10:00:00', 2),
('User created a task', '2025-06-03 11:00:00', 3),
('User left a review', '2024-06-04 12:00:00', 4);

-- 11. Заполнение таблицы notifications
INSERT INTO notifications (message, time, user_id, project_id) VALUES
('System maintainance notice', '2025-04-01 08:00:00', 2, 1),
('New feature deployment', '2025-06-05 09:00:00', 5, 2),
('Project meeting reminder', '2025-06-10 10:00:00', 7, 3);