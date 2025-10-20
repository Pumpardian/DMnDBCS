CREATE EXTENSION IF NOT EXISTS pgcrypto;

--PROJECTS

----CREATE
CREATE OR REPLACE FUNCTION create_project(
	_title VARCHAR,
	_description TEXT,
	_start_date DATE DEFAULT CURRENT_DATE,
	_end_date DATE DEFAULT NULL
)
RETURNS INTEGER AS $$
DECLARE
	_project_id INTEGER;
BEGIN
	INSERT INTO projects
	(title, description, start_date, end_date)
	VALUES
	(_title, _description, _start_date, _end_date)
	RETURNING id INTO _project_id;
	RETURN _project_id;
END; $$ LANGUAGE plpgsql;

----UPDATE
CREATE OR REPLACE FUNCTION update_project(
	_project_id INTEGER,
	_title VARCHAR,
	_description TEXT,
	_start_date DATE,
	_end_date DATE DEFAULT NULL
)
RETURNS VOID AS $$
BEGIN
	UPDATE projects
	SET title = _title,
		description = _description,
		start_date = _start_date,
		end_date = _end_date
	WHERE id = _project_id;
END; $$ LANGUAGE plpgsql;

----DELETE
CREATE OR REPLACE FUNCTION delete_project(
	_project_id INTEGER
)
RETURNS VOID AS $$
BEGIN
	DELETE FROM taskcomments WHERE task_id
		IN (SELECT id FROM tasks WHERE project_id = _project_id);
	DELETE FROM tasks WHERE project_id = _project_id;
	DELETE FROM projectresources WHERE project_id = _project_id;
	DELETE FROM userroles WHERE project_id = _project_id;
	DELETE FROM projects WHERE id = _project_id;
END; $$ LANGUAGE plpgsql;

----GET ALL
CREATE OR REPLACE FUNCTION get_all_projects()
RETURNS TABLE(
	project_id INTEGER,
	project_title VARCHAR,
	project_description TEXT,
	project_start_date DATE,
	project_end_date DATE
) AS $$
BEGIN
	RETURN QUERY
	SELECT id, title, description, start_date, end_date
	FROM projects;
END; $$ LANGUAGE plpgsql;

--TASKS

----CREATE
CREATE OR REPLACE FUNCTION create_task(
	_title VARCHAR,
	_description TEXT,
	_project_id INTEGER,
	_executor_id INTEGER,
	_status INTEGER DEFAULT 1,
	_creation_date DATE DEFAULT CURRENT_DATE,
	_completion_date DATE DEFAULT NULL
)
RETURNS INTEGER AS $$
DECLARE _task_id INTEGER;
BEGIN
	INSERT INTO tasks
	(title, description, status, creation_date, completion_date, project_id, executor_id)
	VALUES
	(_title, _description, _status, _creation_date, _completion_date, _project_id, _executor_id)
	RETURNING id INTO _task_id;
	RETURN _task_id;
END; $$ LANGUAGE plpgsql;

----UPDATE
CREATE OR REPLACE FUNCTION update_task(
	_task_id INTEGER,
	_title VARCHAR,
	_description TEXT,
	_project_id INTEGER,
	_executor_id INTEGER,
	_status INTEGER,
	_creation_date DATE,
	_completion_date DATE DEFAULT NULL
)
RETURNS VOID AS $$
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
END; $$ LANGUAGE plpgsql;

----DELETE
CREATE OR REPLACE FUNCTION delete_task(
	_task_id INTEGER
)
RETURNS VOID AS $$
BEGIN
	DELETE FROM taskcomments WHERE task_id = _task_id;
	DELETE FROM tasks WHERE id = _task_id;
END; $$ LANGUAGE plpgsql;

----GET ALL BY PROJECT
CREATE OR REPLACE FUNCTION get_tasks_by_project(
    _project_id INTEGER
)
RETURNS TABLE(
    task_id INTEGER,
    task_title VARCHAR,
    task_description TEXT,
	task_executor_id INTEGER,
    task_status VARCHAR,
    task_creation_date DATE,
    task_completion_date DATE
) AS $$
BEGIN
    RETURN QUERY
    SELECT tasks.id, tasks.title, tasks.description, tasks.executor_id, taskstatuses.name, tasks.creation_date, tasks.completion_date
    FROM tasks
		LEFT JOIN taskstatuses ON tasks.status = taskstatuses.id
    WHERE tasks.project_id = _project_id;
END;
$$ LANGUAGE plpgsql;

----GET ALL BY EXECUTOR
CREATE OR REPLACE FUNCTION get_tasks_by_executor(
    _executor_id INTEGER
)
RETURNS TABLE(
    task_id INTEGER,
    task_title VARCHAR,
    task_description TEXT,
	task_project_id INTEGER,
    task_status VARCHAR,
    task_creation_date DATE,
    task_completion_date DATE
) AS $$
BEGIN
    RETURN QUERY
    SELECT tasks.id, tasks.title, tasks.description, tasks.project_id, taskstatuses.name, tasks.creation_date, tasks.completion_date
    FROM tasks
		LEFT JOIN taskstatuses ON tasks.status = taskstatuses.id
    WHERE tasks.executor_id = _executor_id;
END;
$$ LANGUAGE plpgsql;

--ROLES

----CREATE
CREATE OR REPLACE FUNCTION create_role(
    _role_name VARCHAR
)
RETURNS INTEGER AS $$
DECLARE
	_role_id INTEGER;
BEGIN
    INSERT INTO roles (name)
    VALUES (_role_name)
	RETURNING id INTO _role_id;
	RETURN _role_id;
END; $$ LANGUAGE plpgsql;

----UPDATE
CREATE OR REPLACE FUNCTION update_role(
	_role_id INTEGER,
	_name VARCHAR
)
RETURNS VOID AS $$
BEGIN
	UPDATE roles
	SET name = _name
	WHERE id = _role_id;
END; $$ LANGUAGE plpgsql;

----DELETE
CREATE OR REPLACE FUNCTION delete_role(
	_role_id INTEGER
)
RETURNS VOID AS $$
BEGIN
    DELETE FROM userroles WHERE role_id = _role_id;
    DELETE FROM roles WHERE id = _role_id;
END; $$ LANGUAGE plpgsql;

----GET ALL
CREATE OR REPLACE FUNCTION get_all_roles()
RETURNS TABLE(
    role_id INTEGER,
    role_name VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT roles.id, roles.name
    FROM roles;
END; $$ LANGUAGE plpgsql;

--TASKSTATUSES

----GET ALL
CREATE OR REPLACE FUNCTION get_all_statuses()
RETURNS TABLE(
    status_id INTEGER,
    status_name VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT taskstatuses.id, taskstatuses.name
    FROM taskstatuses;
END; $$ LANGUAGE plpgsql;

--USERROLES

----CREATE
CREATE OR REPLACE FUNCTION create_userrole(
    _user_id INTEGER,
	_project_id INTEGER,
	_role_id INTEGER
)
RETURNS INTEGER AS $$
DECLARE
	_userrole_id INTEGER;
BEGIN
    INSERT INTO userroles (user_id, role_id, project_id)
    VALUES (_user_id, _role_id, _project_id)
	RETURNING id INTO _userrole_id;
	RETURN _userrole_id;
END; $$ LANGUAGE plpgsql;

----UPDATE
CREATE OR REPLACE FUNCTION update_userrole(
    _user_id INTEGER,
	_project_id INTEGER,
	_role_id INTEGER
)
RETURNS VOID AS $$
BEGIN
    UPDATE userroles
	SET 
		role_id = _role_id
	WHERE user_id = _user_id AND project_id = _project_id;
END; $$ LANGUAGE plpgsql;

----DELETE
CREATE OR REPLACE FUNCTION delete_userrole(
    _user_id INTEGER,
	_project_id INTEGER
)
RETURNS VOID AS $$
BEGIN
	DELETE FROM userroles WHERE user_id = _user_id AND project_id = _project_id;
END; $$ LANGUAGE plpgsql;

----GET ROLE FOR USER IN PROJECT
CREATE OR REPLACE FUNCTION get_user_project_role(
    _user_id INTEGER,
	_project_id INTEGER
)
RETURNS TABLE(
    role_id INTEGER,
    role_name VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT userroles.role_id, roles.name
    FROM userroles
		LEFT JOIN roles ON userroles.role_id = roles.id
    WHERE userroles.user_id = _user_id AND userroles.project_id = _project_id;
END; $$ LANGUAGE plpgsql;

----GET USER ROLES
CREATE OR REPLACE FUNCTION get_user_roles(
    _user_id INTEGER
)
RETURNS TABLE(
    role_id INTEGER,
    role_name VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT userroles.role_id, roles.name
    FROM userroles
		LEFT JOIN roles ON userroles.role_id = roles.id
    WHERE userroles.user_id = _user_id;
END; $$ LANGUAGE plpgsql;

----GET PROJECT MEMBERS WITH ROLES
CREATE OR REPLACE FUNCTION get_project_members(
	_project_id INTEGER
)
RETURNS TABLE(
	user_id INTEGER,
    role_id INTEGER,
    role_name VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT userroles.user_id, userroles.role_id, roles.name
    FROM userroles
		LEFT JOIN roles ON userroles.role_id = roles.id
    WHERE userroles.project_id = _project_id;
END; $$ LANGUAGE plpgsql;

--TASKCOMMENTS

----CREATE
CREATE OR REPLACE FUNCTION create_taskcomment(
    _content TEXT,
    _task_id INTEGER,
    _author_id INTEGER,
	_creation_date DATE DEFAULT CURRENT_DATE
)
RETURNS INTEGER AS $$
DECLARE
	_taskcomment_id INTEGER;
BEGIN
    INSERT INTO taskcomments
	(content, creation_date, task_id, author_id)
    VALUES
	(_content, _creation_date, _task_id, _author_id)
	RETURNING id INTO _taskcomment_id;
	RETURN _taskcomment_id;
END; $$ LANGUAGE plpgsql;

----UPDATE
CREATE OR REPLACE FUNCTION update_taskcomment(
    _task_id INTEGER,
    _author_id INTEGER,
	_content TEXT,
	_creation_date TIMESTAMP
)
RETURNS VOID AS $$
BEGIN
    UPDATE taskcomments
	SET
		content = _content,
		creation_date = _creation_date
	WHERE task_id = _task_id AND author_id = _author_id;
END; $$ LANGUAGE plpgsql;

----UPDATE BY ID
CREATE OR REPLACE FUNCTION update_taskcomment_by_id(
    _taskcomment_id INTEGER,
	_content TEXT,
	_creation_date TIMESTAMP
)
RETURNS VOID AS $$
BEGIN
    UPDATE taskcomments
	SET
		content = _content,
		creation_date = _creation_date
	WHERE id = _taskcomment_id;
END; $$ LANGUAGE plpgsql;

----DELETE
CREATE OR REPLACE FUNCTION delete_taskcomment(
    _taskcomment_id INTEGER
)
RETURNS void AS $$
BEGIN
    DELETE FROM taskcomments WHERE id = _taskcomment_id;
END; $$ LANGUAGE plpgsql;

----GET TASKCOMMENTS
CREATE OR REPLACE FUNCTION get_taskcomments(
    _task_id INTEGER
)
RETURNS TABLE(
    taskcomment_id INTEGER,
    taskcomment_content TEXT,
    comment_creation_date TIMESTAMP,
    author_id INTEGER
) AS $$
BEGIN
    RETURN QUERY
    SELECT taskcomments.id, taskcomments.content, taskcomments.creation_date, taskcomments.author_id
    FROM taskcomments
    WHERE taskcomments.task_id = _task_id;
END; $$ LANGUAGE plpgsql;

--PROJECTRESOURCES

----CREATE
CREATE OR REPLACE FUNCTION create_projectresource(
    _description TEXT,
    _type VARCHAR,
    _project_id INTEGER
)
RETURNS INTEGER AS $$
DECLARE
	_projectresources_id INTEGER;
BEGIN
    INSERT INTO projectresources
	(description, type, project_id)
    VALUES
	(_description, _type, _project_id)
	RETURNING id INTO _projectresources_id;
	RETURN _projectresources_id;
END; $$ LANGUAGE plpgsql;

----UPDATE
CREATE OR REPLACE FUNCTION update_projectresource(
	_projectresources_id INTEGER,
    _description TEXT,
    _type VARCHAR
)
RETURNS VOID AS $$
BEGIN
    UPDATE projectresources
	SET
		description = _description,
		type = _type
	WHERE id = _projectresources_id;
END; $$ LANGUAGE plpgsql;

----DELETE
CREATE OR REPLACE FUNCTION delete_projectresource(
    _projectresources_id INTEGER
)
RETURNS void AS $$
BEGIN
    DELETE FROM projectresources WHERE id = _projectresources_id;
END; $$ LANGUAGE plpgsql;

----GET PROJECTRESOURCES
CREATE OR REPLACE FUNCTION get_projectresources(
    _project_id INTEGER
)
RETURNS TABLE(
    projectresources_id INTEGER,
    projectresources_description TEXT,
    projectresources_type VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT id, description, type
    FROM projectresources
    WHERE project_id = _project_id;
END; $$ LANGUAGE plpgsql;

--NOTIFICATIONS

----CREATE
CREATE OR REPLACE FUNCTION create_notification(
    _message TEXT,
    _time TIMESTAMP,
    _user_id INTEGER,
	_project_id INTEGER
)
RETURNS INTEGER AS $$
DECLARE
	_notification_id INTEGER;
BEGIN
    INSERT INTO notifications
	(message, time, user_id, project_id)
    VALUES
	(_message, _time, _user_id, _project_id)
	RETURNING id INTO _notification_id;
	RETURN _notification_id;
END; $$ LANGUAGE plpgsql;

----UPDATE
CREATE OR REPLACE FUNCTION update_notification(
    _notification_id INTEGER,
	_message TEXT,
    _time TIMESTAMP
)
RETURNS VOID AS $$
BEGIN
	UPDATE notifications
	SET
		message = _message,
		time = _time
	WHERE id = _notification_id;
END; $$ LANGUAGE plpgsql;

----DELETE
CREATE OR REPLACE FUNCTION delete_notification(
    _notification_id INTEGER
)
RETURNS VOID AS $$
BEGIN
    DELETE FROM notifications WHERE id = _notification_id; 
END; $$ LANGUAGE plpgsql;

----GET NOTIFICATIONS FOR USER
CREATE OR REPLACE FUNCTION get_notifications_for_user(
    _user_id INTEGER
)
RETURNS TABLE(
    notification_id INTEGER,
    notification_message TEXT,
    notification_time TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT id, message, time
    FROM notifications
    WHERE user_id = _user_id;
END; $$ LANGUAGE plpgsql;

----GET NOTIFICATIONS FOR PROJECT
CREATE OR REPLACE FUNCTION get_notifications_for_project(
    _project_id INTEGER
)
RETURNS TABLE(
    notification_id INTEGER,
    notification_message TEXT,
    notification_time TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT id, message, time
    FROM notifications
    WHERE project_id = _project_id;
END; $$ LANGUAGE plpgsql;

--USERS

----CREATE
CREATE OR REPLACE FUNCTION create_user_with_profile(
    _name VARCHAR,
    _email VARCHAR,
    _password VARCHAR,
    _phone VARCHAR,
    _address VARCHAR,
    _date_of_birth DATE,
    _profile_picture TEXT
)
RETURNS INTEGER AS $$
DECLARE
    _user_id INTEGER;
BEGIN
    INSERT INTO users
	(name, email, password)
    VALUES
	(_name, _email, crypt(_password, gen_salt('bf')))
    RETURNING id INTO _user_id;

    INSERT INTO userprofiles
	(user_id, phone, address, date_of_birth, profile_picture)
    VALUES
	(_user_id, _phone, _address, _date_of_birth, _profile_picture);

    RETURN _user_id;
END; $$ LANGUAGE plpgsql;

----GET
CREATE OR REPLACE FUNCTION get_user_with_profile(
	_user_id INTEGER
)
RETURNS TABLE(
    user_id INTEGER,
    name VARCHAR,
    email VARCHAR,
    phone VARCHAR,
    address VARCHAR,
    date_of_birth DATE,
    profile_picture TEXT
) AS $$
BEGIN
    RETURN QUERY
    SELECT u.id, u.name, u.email, p.phone, p.address, p.date_of_birth, p.profile_picture
    FROM users u
		LEFT JOIN userprofiles p ON u.id = p.user_id
    WHERE u.id = _user_id;
END; $$ LANGUAGE plpgsql;

----UPDATE
CREATE OR REPLACE FUNCTION update_user_and_profile(
    _user_id INTEGER,
    _name VARCHAR,
    _email VARCHAR,
    _password VARCHAR,
    _phone VARCHAR,
    _address VARCHAR,
    _date_of_birth DATE,
    _profile_picture TEXT
)
RETURNS VOID AS $$
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

    UPDATE userprofiles
    SET
		phone = _phone,
        address = _address,
        date_of_birth = _date_of_birth,
        profile_picture = _profile_picture
    WHERE user_id = _user_id;
END; $$ LANGUAGE plpgsql;

----DELETE
CREATE OR REPLACE FUNCTION delete_user(
	_user_id INTEGER
)
RETURNS VOID AS $$
BEGIN
    DELETE FROM userroles WHERE user_id = _user_id;
    DELETE FROM userprofiles WHERE user_id = _user_id;
    DELETE FROM users WHERE id = _user_id;
END; $$ LANGUAGE plpgsql;

----AUTHENTICATE
CREATE OR REPLACE FUNCTION authenticate_user(
    _email VARCHAR,
    _password VARCHAR
)
RETURNS TABLE(
    user_id INTEGER,
    user_name VARCHAR
) AS $$
DECLARE
	_user_id INTEGER;
    _stored_password TEXT;
BEGIN
    SELECT id, password INTO _user_id, _stored_password
    FROM users
    WHERE email = _email;

    IF user_id IS NOT NULL AND _stored_password = crypt(_password, _stored_password) THEN
        RETURN QUERY SELECT user_id, (SELECT name FROM users WHERE id = _user_id);
    ELSE
        RETURN QUERY SELECT NULL::INTEGER, NULL::VARCHAR;
    END IF;
END; $$ LANGUAGE plpgsql;

--GET ALL LOGS
CREATE OR REPLACE FUNCTION get_all_logs()
RETURNS TABLE(
	id INTEGER,
	action TEXT,
	date TIMESTAMP,
	user_name VARCHAR
) AS $$
BEGIN
	RETURN QUERY
	SELECT
		logs.id,
		logs.action,
		logs.date,
		users.name
	FROM logs
		LEFT JOIN users ON users.id = logs.user_id
	ORDER BY logs.date DESC;
END; $$ LANGUAGE plpgsql;