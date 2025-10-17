--AUTOMATION: PROJECT COMPLETION DATE ON TASKS CHANGE & TASK COMPLETION DATE ON STATUS CHANGE
CREATE OR REPLACE FUNCTION completion_dates_automation()
RETURNS TRIGGER AS $$
DECLARE
	status_name VARCHAR(50);
BEGIN
	SELECT name INTO status_name FROM taskstatuses WHERE id = NEW.status;

	IF status_name IN ('Cancelled', 'Completed') AND NEW.completion_date IS NULL
	THEN
		NEW.completion_date = CURRENT_DATE;
	ELSIF status_name NOT IN ('Cancelled', 'Completed') AND NEW.completion_date IS NOT NULL
	THEN
		NEW.completion_date = NULL;
	END IF;

	IF NOT EXISTS
	(
		SELECT 1
		FROM tasks
			JOIN taskstatuses ON tasks.status = taskstatuses.id
		WHERE
			project_id = NEW.project_id
			AND taskstatuses.name NOT IN ('Cancelled', 'Completed')
	)
	THEN
		UPDATE projects SET
			end_date = CURRENT_DATE
		WHERE id = NEW.project_id;
	ELSE
		UPDATE projects SET
			end_date = NULL
		WHERE id = NEW.project_id;
	END IF;
	
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_update_project_completion_date
BEFORE INSERT OR UPDATE ON tasks
FOR EACH ROW
EXECUTE FUNCTION completion_dates_automation();

--LOGGING

----TASK

------INSERTION
CREATE OR REPLACE FUNCTION log_task_insert()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Created task with id '||NEW.id||
	' and title '||NEW.title||
	' for project with id '||NEW.project_id,
	CURRENT_TIMESTAMP, NEW.executor_id)
	RETURN NEW
END; $$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_log_task_insert
AFTER INSERT ON tasks
FOR EACH ROW
EXECUTE log_task_insert();

------UPDATE
CREATE OR REPLACE FUNCTION log_task_update()
RETURNS TRIGGER AS $$
DECLARE
	old_status VARCHAR(50);
	new_status VARCHAR(50);
BEGIN
	SELECT name INTO old_status FROM taskstatuses WHERE id = OLD.status;
	SELECT name INTO new_status FROM taskstatuses WHERE id = NEW.status;

	INSERT INTO logs (action, date, user_id)
	VALUES ('Updated task with id '||NEW.id||
	CASE
		WHEN OLD.status != NEW.status THEN
			', status changed: '||old_status||' to '||new_status
		WHEN OLD.title != NEW.title THEN
			', title changed: '||OLD.title||' to '||NEW.title
		WHEN OLD.description != NEW.description THEN
			', description changed'
		ELSE
			', some fields changed'
	END,
	CURRENT_TIMESTAMP, NEW.executor_id)
	RETURN NEW
END; $$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_log_task_update
AFTER UPDATE ON tasks
FOR EACH ROW
EXECUTE log_task_update();

------DELETE
CREATE OR REPLACE FUNCTION log_task_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Deleted task with id '||OLD.id||
	' and title '||OLD.title,
	CURRENT_TIMESTAMP, OLD.executor_id)
	RETURN OLD
END; $$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_log_task_delete
AFTER DELETE ON tasks
FOR EACH ROW
EXECUTE log_task_delete();

----PROJECT

------INSERTION
CREATE OR REPLACE FUNCTION log_project_insert()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Created project with id '||NEW.id||
	' and title '||NEW.title||,
	CURRENT_TIMESTAMP, NULL)
	RETURN NEW
END; $$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_log_project_insert
AFTER INSERT ON projects
FOR EACH ROW
EXECUTE log_project_insert();

------UPDATE
CREATE OR REPLACE FUNCTION log_project_update()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Updated project with id '||NEW.id||
	CASE
		WHEN OLD.title != NEW.title THEN
			', title changed: '||OLD.title||' to '||NEW.title
		WHEN OLD.description != NEW.description THEN
			', description changed'
		ELSE
			', some fields changed'
	END,
	CURRENT_TIMESTAMP, NULL)
	RETURN NEW
END; $$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_log_project_update
AFTER UPDATE ON projects
FOR EACH ROW
EXECUTE log_project_update();

------DELETE
CREATE OR REPLACE FUNCTION log_project_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Deleted project with id '||OLD.id
	||' and title '||OLD.title,
	CURRENT_TIMESTAMP, NULL)
	RETURN OLD
END; $$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_log_project_delete
AFTER UPDATE ON projects
FOR EACH ROW
EXECUTE log_project_update();

----TASKCOMMENTS

------INSERTION
CREATE OR REPLACE FUNCTION log_taskcomment_insert()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Created taskcomment with id '||NEW.id||
	' by '||(SELECT name FROM users WHEN users.id = NEW.author_id),
	CURRENT_TIMESTAMP, NEW.author_id)
	RETURN NEW
END; $$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_log_taskcomment_insert
AFTER INSERT ON taskcomments
FOR EACH ROW
EXECUTE log_taskcomment_insert();

------UPDATE
CREATE OR REPLACE FUNCTION log_taskcomment_update()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Updated taskcomment with id '||NEW.id||
	CASE
		WHEN OLD.content != NEW.content THEN
			', content changed'
		ELSE
			', some fields changed'
	END,
	CURRENT_TIMESTAMP, NEW.author_id)
	RETURN NEW
END; $$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_log_taskcomment_update
AFTER UPDATE ON taskcomments
FOR EACH ROW
EXECUTE log_taskcomment_update();

------DELETE
CREATE OR REPLACE FUNCTION log_taskcomment_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Deleted taskcomment with id '||OLD.id||
	' by '||(SELECT name FROM users WHEN users.id = OLD.author_id),
	CURRENT_TIMESTAMP, OLD.author_id)
	RETURN OLD
END; $$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_log_taskcomment_delete
AFTER UPDATE ON taskcomments
FOR EACH ROW
EXECUTE log_taskcomment_update();

--TODO:

----PROJECTRESOURCES

----NOTIFICATIONS