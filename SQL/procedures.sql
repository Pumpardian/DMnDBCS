CREATE EXTENSION IF NOT EXISTS pgcrypto;

--PROJECTS

----CREATE
CREATE OR REPLACE PROCEDURE create_project(
    _title VARCHAR,
    _description TEXT,
    _start_date DATE DEFAULT CURRENT_DATE,
    _end_date DATE DEFAULT NULL,
    INOUT _project_id INTEGER DEFAULT NULL
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO projects
    (title, description, start_date, end_date)
    VALUES
    (_title, _description, _start_date, _end_date)
    RETURNING id INTO _project_id;
END; $$;

----UPDATE
CREATE OR REPLACE PROCEDURE update_project(
    _project_id INTEGER,
    _title VARCHAR,
    _description TEXT,
    _start_date DATE,
    _end_date DATE DEFAULT NULL
)
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE projects
    SET title = _title,
        description = _description,
        start_date = _start_date,
        end_date = _end_date
    WHERE id = _project_id;
END; $$;

----DELETE
CREATE OR REPLACE PROCEDURE delete_project(
    _project_id INTEGER
)
LANGUAGE plpgsql AS $$
BEGIN
    DELETE FROM taskcomments WHERE task_id
        IN (SELECT id FROM tasks WHERE project_id = _project_id);
    DELETE FROM tasks WHERE project_id = _project_id;
    DELETE FROM projectresources WHERE project_id = _project_id;
    DELETE FROM userroles WHERE project_id = _project_id;
    DELETE FROM projects WHERE id = _project_id;
END; $$;

----GET ALL
CREATE OR REPLACE PROCEDURE get_all_projects(
    INOUT _ref refcursor DEFAULT 'projects_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT id AS project_id, title AS project_title, description AS project_description, 
           start_date AS project_start_date, end_date AS project_end_date
    FROM projects;
END; $$;

----GET
CREATE OR REPLACE PROCEDURE get_project(
	_project_id INTEGER,
    INOUT _ref refcursor DEFAULT 'projects_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT id AS project_id, title AS project_title, description AS project_description, 
           start_date AS project_start_date, end_date AS project_end_date
    FROM projects
	WHERE id = _project_id;
END; $$;

----GET LATEST
CREATE OR REPLACE PROCEDURE get_latest_project(
    INOUT _ref refcursor DEFAULT 'projects_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT id AS project_id, title AS project_title, description AS project_description, 
           start_date AS project_start_date, end_date AS project_end_date
    FROM projects
	ORDER BY id DESC
    LIMIT 1;
END; $$;

--TASKS

----CREATE
CREATE OR REPLACE PROCEDURE create_task(
    _title VARCHAR,
    _description TEXT,
    _project_id INTEGER,
    _executor_id INTEGER,
    _status INTEGER DEFAULT 1,
    _creation_date DATE DEFAULT CURRENT_DATE,
    _completion_date DATE DEFAULT NULL,
    INOUT _task_id INTEGER DEFAULT NULL
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO tasks
    (title, description, status, creation_date, completion_date, project_id, executor_id)
    VALUES
    (_title, _description, _status, _creation_date, _completion_date, _project_id, _executor_id)
    RETURNING id INTO _task_id;
END; $$;

----UPDATE
CREATE OR REPLACE PROCEDURE update_task(
    _task_id INTEGER,
    _title VARCHAR,
    _description TEXT,
    _project_id INTEGER,
    _executor_id INTEGER,
    _status INTEGER,
    _creation_date DATE,
    _completion_date DATE DEFAULT NULL
)
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE tasks
    SET title = _title,
        description = _description,
        project_id = _project_id,
        executor_id = _executor_id,
        status = _status,
        creation_date = _creation_date,
        completion_date = _completion_date
    WHERE id = _task_id;
END; $$;

----DELETE
CREATE OR REPLACE PROCEDURE delete_task(
    _task_id INTEGER
)
LANGUAGE plpgsql AS $$
BEGIN
    DELETE FROM taskcomments WHERE task_id = _task_id;
    DELETE FROM tasks WHERE id = _task_id;
END; $$;

----GET ALL BY PROJECT
CREATE OR REPLACE PROCEDURE get_tasks_by_project(
    _project_id INTEGER,
    INOUT _ref refcursor DEFAULT 'tasks_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT tasks.id AS task_id, tasks.title AS task_title, tasks.description AS task_description, 
           tasks.executor_id AS task_executor_id, taskstatuses.id AS task_status, taskstatuses.name AS task_status_name,
           tasks.creation_date AS task_creation_date, tasks.completion_date AS task_completion_date
    FROM tasks
        LEFT JOIN taskstatuses ON tasks.status = taskstatuses.id
    WHERE tasks.project_id = _project_id;
END; $$;

----GET ALL BY EXECUTOR
CREATE OR REPLACE PROCEDURE get_tasks_by_executor(
    _executor_id INTEGER,
    INOUT _ref refcursor DEFAULT 'tasks_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT tasks.id AS task_id, tasks.title AS task_title, tasks.description AS task_description, 
           tasks.project_id AS task_project_id, taskstatuses.id AS task_status, taskstatuses.name AS task_status_name, 
           tasks.creation_date AS task_creation_date, tasks.completion_date AS task_completion_date
    FROM tasks
        LEFT JOIN taskstatuses ON tasks.status = taskstatuses.id
    WHERE tasks.executor_id = _executor_id;
END; $$;

----GET
CREATE OR REPLACE PROCEDURE get_task(
    _task_id INTEGER,
    INOUT _ref refcursor DEFAULT 'tasks_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT tasks.id AS task_id, tasks.title AS task_title, tasks.description AS task_description, 
           tasks.project_id AS task_project_id, tasks.executor_id AS task_executor_id, taskstatuses.id AS task_status, taskstatuses.name AS task_status_name,
           tasks.creation_date AS task_creation_date, tasks.completion_date AS task_completion_date
    FROM tasks
        LEFT JOIN taskstatuses ON tasks.status = taskstatuses.id
    WHERE tasks.id = _task_id;
END; $$;

--ROLES

----CREATE
CREATE OR REPLACE PROCEDURE create_role(
    _role_name VARCHAR,
    INOUT _role_id INTEGER DEFAULT NULL
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO roles (name)
    VALUES (_role_name)
    RETURNING id INTO _role_id;
END; $$;

----UPDATE
CREATE OR REPLACE PROCEDURE update_role(
    _role_id INTEGER,
    _name VARCHAR
)
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE roles
    SET name = _name
    WHERE id = _role_id;
END; $$;

----DELETE
CREATE OR REPLACE PROCEDURE delete_role(
    _role_id INTEGER
)
LANGUAGE plpgsql AS $$
BEGIN
    DELETE FROM userroles WHERE role_id = _role_id;
    DELETE FROM roles WHERE id = _role_id;
END; $$;

----GET ALL
CREATE OR REPLACE PROCEDURE get_all_roles(
    INOUT _ref refcursor DEFAULT 'roles_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT roles.id AS role_id, roles.name AS role_name
    FROM roles;
END; $$;

--TASKSTATUSES

----GET ALL
CREATE OR REPLACE PROCEDURE get_all_statuses(
    INOUT _ref refcursor DEFAULT 'statuses_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT taskstatuses.id AS status_id, taskstatuses.name AS status_name
    FROM taskstatuses;
END; $$;

--USERROLES

----CREATE
CREATE OR REPLACE PROCEDURE create_userrole(
    _user_id INTEGER,
    _project_id INTEGER,
    _role_id INTEGER
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO userroles (user_id, role_id, project_id)
    VALUES (_user_id, _role_id, _project_id);
END; $$;

----UPDATE
CREATE OR REPLACE PROCEDURE update_userrole(
    _user_id INTEGER,
    _project_id INTEGER,
    _role_id INTEGER
)
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE userroles
    SET 
        role_id = _role_id
    WHERE user_id = _user_id AND project_id = _project_id;
END; $$;

----DELETE
CREATE OR REPLACE PROCEDURE delete_userrole(
    _user_id INTEGER,
    _project_id INTEGER
)
LANGUAGE plpgsql AS $$
BEGIN
    DELETE FROM userroles WHERE user_id = _user_id AND project_id = _project_id;
END; $$;

----GET ROLE FOR USER IN PROJECT
CREATE OR REPLACE PROCEDURE get_user_project_role(
    _user_id INTEGER,
    _project_id INTEGER,
    INOUT _ref refcursor DEFAULT 'roles_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT userroles.role_id, roles.name AS role_name
    FROM userroles
        LEFT JOIN roles ON userroles.role_id = roles.id
    WHERE userroles.user_id = _user_id AND userroles.project_id = _project_id;
END; $$;

----GET USER ROLES IN PROJECT
CREATE OR REPLACE PROCEDURE get_userroles_in_project(
	_project_id INTEGER,
    INOUT _ref refcursor DEFAULT 'roles_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT userroles.role_id, roles.name AS role_name
    FROM userroles
        LEFT JOIN roles ON userroles.role_id = roles.id
    WHERE userroles.project_id = _project_id;
END; $$;

----GET PROJECT MEMBERS WITH ROLES
CREATE OR REPLACE PROCEDURE get_project_members(
    _project_id INTEGER,
    INOUT _ref refcursor DEFAULT 'members_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT userroles.user_id, userroles.role_id, users.name AS user_name, roles.name AS role_name
    FROM userroles
        LEFT JOIN roles ON userroles.role_id = roles.id
		LEFT JOIN users ON userroles.user_id = users.id
    WHERE userroles.project_id = _project_id;
END; $$;

--TASKCOMMENTS

----CREATE
CREATE OR REPLACE PROCEDURE create_taskcomment(
    _content TEXT,
    _task_id INTEGER,
    _author_id INTEGER,
    _creation_date DATE DEFAULT CURRENT_DATE,
    INOUT _taskcomment_id INTEGER DEFAULT NULL
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO taskcomments
    (content, creation_date, task_id, author_id)
    VALUES
    (_content, _creation_date, _task_id, _author_id)
    RETURNING id INTO _taskcomment_id;
END; $$;

----UPDATE
CREATE OR REPLACE PROCEDURE update_taskcomment(
    _taskcomment_id INTEGER,
    _content TEXT,
    _creation_date TIMESTAMP
)
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE taskcomments
    SET
        content = _content,
        creation_date = _creation_date
    WHERE id = _taskcomment_id;
END; $$;

----DELETE
CREATE OR REPLACE PROCEDURE delete_taskcomment(
    _taskcomment_id INTEGER
)
LANGUAGE plpgsql AS $$
BEGIN
    DELETE FROM taskcomments WHERE id = _taskcomment_id;
END; $$;

----GET
CREATE OR REPLACE PROCEDURE get_taskcomments(
    _task_id INTEGER,
    INOUT _ref refcursor DEFAULT 'comments_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT taskcomments.id AS taskcomment_id, taskcomments.content AS taskcomment_content, 
           taskcomments.creation_date AS comment_creation_date, taskcomments.author_id
    FROM taskcomments
    WHERE taskcomments.task_id = _task_id;
END; $$;

----GET BY ID
CREATE OR REPLACE PROCEDURE get_taskcomment(
    _id INTEGER,
    INOUT _ref refcursor DEFAULT 'comments_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT taskcomments.id AS taskcomment_id, taskcomments.content AS taskcomment_content, 
           taskcomments.creation_date AS comment_creation_date, taskcomments.task_id, taskcomments.author_id
    FROM taskcomments
    WHERE taskcomments.id = _id;
END; $$;

--PROJECTRESOURCES

----CREATE
CREATE OR REPLACE PROCEDURE create_projectresource(
    _description TEXT,
    _type VARCHAR,
    _project_id INTEGER,
    INOUT _projectresources_id INTEGER DEFAULT NULL
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO projectresources
    (description, type, project_id)
    VALUES
    (_description, _type, _project_id)
    RETURNING id INTO _projectresources_id;
END; $$;

----UPDATE
CREATE OR REPLACE PROCEDURE update_projectresource(
    _projectresources_id INTEGER,
    _description TEXT,
    _type VARCHAR
)
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE projectresources
    SET
        description = _description,
        type = _type
    WHERE id = _projectresources_id;
END; $$;

----DELETE
CREATE OR REPLACE PROCEDURE delete_projectresource(
    _projectresources_id INTEGER
)
LANGUAGE plpgsql AS $$
BEGIN
    DELETE FROM projectresources WHERE id = _projectresources_id;
END; $$;

----GET PROJECTRESOURCES
CREATE OR REPLACE PROCEDURE get_projectresources(
    _project_id INTEGER,
    INOUT _ref refcursor DEFAULT 'resources_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT id AS projectresources_id, description AS projectresources_description, 
           type AS projectresources_type
    FROM projectresources
    WHERE project_id = _project_id;
END; $$;

----GET BY ID
CREATE OR REPLACE PROCEDURE get_projectresource(
    _id INTEGER,
    INOUT _ref refcursor DEFAULT 'resources_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT id AS projectresources_id, description AS projectresources_description, 
           type AS projectresources_type, project_id AS projectresources_projectid
    FROM projectresources
    WHERE id = _id;
END; $$;

--NOTIFICATIONS

----CREATE
CREATE OR REPLACE PROCEDURE create_notification(
    _message TEXT,
    _time TIMESTAMP,
    _user_id INTEGER,
    _project_id INTEGER,
    INOUT _notification_id INTEGER DEFAULT NULL
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO notifications
    (message, time, user_id, project_id)
    VALUES
    (_message, _time, _user_id, _project_id)
    RETURNING id INTO _notification_id;
END; $$;

----UPDATE
CREATE OR REPLACE PROCEDURE update_notification(
    _notification_id INTEGER,
    _message TEXT,
    _time TIMESTAMP
)
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE notifications
    SET
        message = _message,
        time = _time
    WHERE id = _notification_id;
END; $$;

----DELETE
CREATE OR REPLACE PROCEDURE delete_notification(
    _notification_id INTEGER
)
LANGUAGE plpgsql AS $$
BEGIN
    DELETE FROM notifications WHERE id = _notification_id; 
END; $$;

----GET BY ID
CREATE OR REPLACE PROCEDURE get_notification(
    _id INTEGER,
    INOUT _ref refcursor DEFAULT 'notifications_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT id AS notification_id, message AS notification_message, 
           time AS notification_time, user_id, project_id
    FROM notifications
    WHERE id = _id;
END; $$;

----GET NOTIFICATIONS FOR USER
CREATE OR REPLACE PROCEDURE get_notifications_for_user(
    _user_id INTEGER,
    INOUT _ref refcursor DEFAULT 'notifications_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT id AS notification_id, message AS notification_message, 
           time AS notification_time, user_id, project_id
    FROM notifications
    WHERE user_id = _user_id
	UNION
	SELECT id AS notification_id, message AS notification_message, 
           time AS notification_time, user_id, project_id
    FROM notifications
    WHERE project_id IN
	(
        SELECT ur.project_id 
        FROM userroles ur 
        WHERE ur.user_id = _user_id
    );
END; $$;

----GET NOTIFICATIONS FOR PROJECT
CREATE OR REPLACE PROCEDURE get_notifications_for_project(
    _project_id INTEGER,
    INOUT _ref refcursor DEFAULT 'notifications_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT id AS notification_id, message AS notification_message, 
           time AS notification_time
    FROM notifications
    WHERE project_id = _project_id;
END; $$;

--USERS

----CREATE
CREATE OR REPLACE PROCEDURE create_user(
    _name VARCHAR,
    _email VARCHAR,
    _password VARCHAR,
    INOUT _user_id INTEGER DEFAULT NULL
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO users
    (name, email, password)
    VALUES
    (_name, _email, crypt(_password, gen_salt('bf')))
    RETURNING id INTO _user_id;
END; $$;

----GET ALL
CREATE OR REPLACE PROCEDURE get_users(
    INOUT _ref refcursor DEFAULT 'user_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT u.id, u.name, u.email
    FROM users u;
END; $$;

----GET USERS THAT ARE NOT IN PROJECT
CREATE OR REPLACE PROCEDURE get_users_not_in_project(
    _project_id INTEGER,
    INOUT _ref refcursor DEFAULT 'user_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT u.id, u.name, u.email
    FROM users u
    WHERE u.id NOT IN
	(
        SELECT ur.user_id 
        FROM userroles ur 
        WHERE ur.project_id = _project_id
    );
END; $$;

----GET USERS THAT ARE IN PROJECT
CREATE OR REPLACE PROCEDURE get_users_in_project(
    _project_id INTEGER,
    INOUT _ref refcursor DEFAULT 'user_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT u.id, u.name, u.email
    FROM users u
    WHERE u.id IN
	(
        SELECT ur.user_id 
        FROM userroles ur 
        WHERE ur.project_id = _project_id
    );
END; $$;

----GET
CREATE OR REPLACE PROCEDURE get_user(
    _user_id INTEGER,
    INOUT _ref refcursor DEFAULT 'user_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
	SELECT id, name, email
    FROM users
    WHERE id = _user_id;
END; $$;

----GET BY EMAIL
CREATE OR REPLACE PROCEDURE get_user_by_email(
    _email VARCHAR,
    INOUT _ref refcursor DEFAULT 'user_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
	SELECT id, name, email
    FROM users
    WHERE email = _email;
END; $$;

----UPDATE
CREATE OR REPLACE PROCEDURE update_user(
    _user_id INTEGER,
    _name VARCHAR,
    _email VARCHAR,
    _password VARCHAR
)
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE users
    SET
        name = _name,
        email = _email,
        password =
        CASE
            WHEN _password IS NOT NULL THEN
                crypt(_password, gen_salt('bf'))
            ELSE
                password
        END
    WHERE id = _user_id;
END; $$;

----DELETE
CREATE OR REPLACE PROCEDURE delete_user(
    _user_id INTEGER
)
LANGUAGE plpgsql AS $$
BEGIN
	DELETE FROM logs WHERE user_id = _user_id;
    DELETE FROM userroles WHERE user_id = _user_id;
    DELETE FROM userprofiles WHERE user_id = _user_id;
    DELETE FROM users WHERE id = _user_id;
END; $$;

----AUTHENTICATE
CREATE OR REPLACE PROCEDURE authenticate_user(
    _email VARCHAR,
    _password VARCHAR,
    INOUT _ref refcursor DEFAULT 'auth_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT u.id AS user_id, u.name AS user_name
    FROM users u
    WHERE u.email = _email AND u.password = crypt(_password, u.password);
END; $$;

--USERPROFILES

----CREATE
CREATE OR REPLACE PROCEDURE create_userprofile(
    _phone VARCHAR,
    _address VARCHAR,
    _date_of_birth DATE,
    _profile_picture TEXT,
    _user_id INTEGER
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO userprofiles
    (user_id, phone, address, date_of_birth, profile_picture)
    VALUES
    (_user_id, _phone, _address, _date_of_birth, _profile_picture);
END; $$;

----GET
CREATE OR REPLACE PROCEDURE get_userprofile(
	_user_id INTEGER,
    INOUT _ref refcursor DEFAULT 'userprofile_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT p.user_id, p.phone, p.address, 
           p.date_of_birth, p.profile_picture
    FROM userprofiles p
	WHERE p.user_id = _user_id;
END; $$;

----GET BY PHONE
CREATE OR REPLACE PROCEDURE get_userprofile_by_phone(
    _phone VARCHAR,
    INOUT _ref refcursor DEFAULT 'userprofile_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT p.user_id, p.phone, p.address, 
           p.date_of_birth, p.profile_picture
    FROM userprofiles p
    WHERE p.phone = _phone;
END; $$;

----UPDATE
CREATE OR REPLACE PROCEDURE update_profile(
    _user_id INTEGER,
    _phone VARCHAR,
    _address VARCHAR,
    _date_of_birth DATE,
    _profile_picture TEXT
)
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE userprofiles
    SET
        phone = _phone,
        address = _address,
        date_of_birth = _date_of_birth,
        profile_picture = _profile_picture
    WHERE user_id = _user_id;
END; $$;

--GET ALL LOGS
CREATE OR REPLACE PROCEDURE get_all_logs(
    INOUT _ref refcursor DEFAULT 'logs_cursor'
)
LANGUAGE plpgsql AS $$
BEGIN
    OPEN _ref FOR
    SELECT
        logs.id,
        logs.action,
        logs.date,
        users.name AS user_name
    FROM logs
        LEFT JOIN users ON users.id = logs.user_id
    ORDER BY logs.date DESC;
END; $$;