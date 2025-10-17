SELECT *
FROM users;
--
SELECT *
FROM users
WHERE email = 'helena.read@example.com';
--
INSERT INTO users (name, email, password)
VALUES (
        'Dmitry Olezhen',
        'dmitry.olezhen@example.com',
        'uniquepassword'
    );
--
UPDATE users
SET password = 'newpassword'
WHERE email = 'helena.read@example.com';
--
DELETE FROM users
WHERE id = 11;
--
SELECT *
FROM projects;
--
INSERT INTO projects (title, description, start_date, end_date)
VALUES (
        'New Project',
        'New project description',
        '2025-09-01',
        NULL
    );
--
UPDATE projects
SET end_date = '2025-08-30'
WHERE id = 1;
--
DELETE FROM projects
WHERE id = 6;
--
SELECT *
FROM tasks
WHERE status = 3;
--
INSERT INTO tasks (
        title,
        description,
        status,
        creation_date,
        project_id,
        executor_id
    )
VALUES (
        'New Task',
        'New Task description',
        1,
        NOW(),
        1,
        2
    );
--
UPDATE tasks
SET status = 4,
    completion_date = NOW()
WHERE id = 15;
--
DELETE FROM tasks
WHERE id = 15;
--
SELECT *
FROM taskcomments
WHERE task_id = 1;
--
INSERT INTO taskcomments (content, creation_date, task_id, author_id)
VALUES ('Task commentary', NOW(), 1, 2);
--
DELETE FROM taskcomments
WHERE id = 11;
--
SELECT *
FROM notifications
WHERE '1' = ANY(user_id);
--
INSERT INTO notifications (message, time, user_id, project_id)
VALUES ('New notification', NOW(), 2, 1);
--
DELETE FROM notifications
WHERE id = 4;
--
SELECT *
FROM projectresources
WHERE project_id = 1;
--
INSERT INTO projectresources (description, type, project_id)
VALUES ('New server', 'Hardware', 1);
--
DELETE FROM projectresources
WHERE id = 8;
--
SELECT project_id,
    COUNT(*) AS task_count
FROM tasks
GROUP BY project_id;
--
SELECT tasks.id,
    tasks.title,
    projects.title AS project_title
FROM tasks
    INNER JOIN projects ON tasks.project_id = projects.id;
--
TRUNCATE TABLE userroles,
taskcomments,
tasks,
taskstatuses,
projectresources,
notifications,
logs,
projects,
roles,
users,
userprofiles RESTART IDENTITY CASCADE;
--
DROP SCHEMA IF EXISTS public CASCADE;
CREATE SCHEMA public;