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
		WHERE id = NEW.project_id AND end_date IS NOT NULL;
	END IF;
	
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_update_project_completion_date
BEFORE INSERT OR UPDATE ON tasks
FOR EACH ROW
EXECUTE FUNCTION completion_dates_automation();

--NOTIFICATIONS

----TASKCOMMENTS

------INSERT
CREATE OR REPLACE FUNCTION notify_taskcomments_insert()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO notifications (message, time, user_id, project_id)
	VALUES ('There are new comment for your task '||
	(SELECT title FROM tasks WHERE id = NEW.task_id)||
	' from user '||
	(SELECT name FROM users WHERE id = NEW.author_id),
	CURRENT_TIMESTAMP, (SELECT executor_id FROM tasks WHERE id = NEW.task_id), NULL);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_notify_taskcomments_insert
AFTER INSERT ON taskcomments
FOR EACH ROW
EXECUTE FUNCTION notify_taskcomments_insert();

------UPDATE
CREATE OR REPLACE FUNCTION notify_taskcomments_update()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO notifications (message, time, user_id, project_id)
	VALUES ('A comment for your task '||
	(SELECT title FROM tasks WHERE id = NEW.task_id)||
	' from user '||
	(SELECT name FROM users WHERE id = NEW.author_id)||
	' was updated',
	CURRENT_TIMESTAMP, (SELECT executor_id FROM tasks WHERE id = NEW.task_id), NULL);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_notify_taskcomments_update
AFTER UPDATE ON taskcomments
FOR EACH ROW
EXECUTE FUNCTION notify_taskcomments_update();

------DELETE
CREATE OR REPLACE FUNCTION notify_taskcomments_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO notifications (message, time, user_id, project_id)
	VALUES ('A comment for your task '||
	(SELECT title FROM tasks WHERE id = OLD.task_id)||
	' from user '||
	(SELECT name FROM users WHERE id = OLD.author_id)||
	' was deleted',
	CURRENT_TIMESTAMP, (SELECT executor_id FROM tasks WHERE id = OLD.task_id), NULL);
	RETURN OLD;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_notify_taskcomments_delete
AFTER DELETE ON taskcomments
FOR EACH ROW
EXECUTE FUNCTION notify_taskcomments_delete();

----PROJECTRESOURCES

------INSERT
CREATE OR REPLACE FUNCTION notify_projectresources_insert()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO notifications (message, time, user_id, project_id)
	VALUES ('There are new resource for your disposal in project'||
	(SELECT title FROM projects WHERE id = NEW.project_id)||
	' of type '||NEW.type,
	CURRENT_TIMESTAMP, NULL, NEW.project_id);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_notify_projectresources_insert
AFTER INSERT ON projectresources
FOR EACH ROW
EXECUTE FUNCTION notify_projectresources_insert();

------UPDATE
CREATE OR REPLACE FUNCTION notify_projectresources_update()
RETURNS TRIGGER AS $$
BEGIN
	CASE
		WHEN OLD.type != NEW.type THEN
			INSERT INTO notifications (message, time, user_id, project_id)
			VALUES ('The resource in project '||
			(SELECT title FROM projects WHERE id = NEW.project_id)||
			' of type '||OLD.type||' has been replaced with'||NEW.type,
			CURRENT_TIMESTAMP, NULL, NEW.project_id);
			
		WHEN OLD.project_id != NEW.project_id THEN
			INSERT INTO notifications (message, time, user_id, project_id)
			VALUES ('The resource of type '||
			OLD.type||' has been unsassigned from project '||
			(SELECT title FROM projects WHERE id = OLD.project_id),
			CURRENT_TIMESTAMP, NULL, OLD.project_id);
			
			INSERT INTO notifications (message, time, user_id, project_id)
			VALUES ('The resource of type '||
			OLD.type||' has been assigned to project '||
			(SELECT title FROM projects WHERE id = NEW.project_id),
			CURRENT_TIMESTAMP, NULL, NEW.project_id);
		
		ELSE
			INSERT INTO notifications (message, time, user_id, project_id)
			VALUES ('The resource of type '||
			OLD.type||' has been updated ',
			CURRENT_TIMESTAMP, NULL, NEW.project_id);
	END CASE;
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_notify_projectresources_update
AFTER UPDATE ON projectresources
FOR EACH ROW
EXECUTE FUNCTION notify_projectresources_update();

------DELETE
CREATE OR REPLACE FUNCTION notify_projectresources_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO notifications (message, time, user_id, project_id)
	VALUES ('The resource of type '||
	OLD.type||' has been depleted in project '||
	(SELECT title FROM projects WHERE id = OLD.project_id),
	CURRENT_TIMESTAMP, NULL, OLD.project_id);
	RETURN OLD;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_notify_projectresources_delete
AFTER DELETE ON projectresources
FOR EACH ROW
EXECUTE FUNCTION notify_projectresources_delete();

----USERROLES

------INSERT & UPDATE
CREATE OR REPLACE FUNCTION notify_userroles_insert_and_update()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO notifications (message, time, user_id, project_id)
	VALUES ('Your role in project '||
	(SELECT title FROM projects WHERE id = NEW.project_id)||
	' has been set to '||(SELECT name FROM roles WHERE id = NEW.role_id),
	CURRENT_TIMESTAMP, NEW.user_id, NULL);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_notify_userroles_insert_and_update
AFTER INSERT OR UPDATE ON userroles
FOR EACH ROW
EXECUTE FUNCTION notify_userroles_insert_and_update();

------DELETE
CREATE OR REPLACE FUNCTION notify_userroles_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO notifications (message, time, user_id, project_id)
	VALUES ('You have been unassigned from project '||
	(SELECT title FROM projects WHERE id = OLD.project_id),
	CURRENT_TIMESTAMP, OLD.user_id, NULL);
	RETURN OLD;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_notify_userroles_delete
AFTER DELETE ON userroles
FOR EACH ROW
EXECUTE FUNCTION notify_userroles_delete();

----TASK ASSIGNMENT

------INSERT
CREATE OR REPLACE FUNCTION notify_task_insert()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO notifications (message, time, user_id, project_id)
	VALUES ('You have been assigned to task '||NEW.title,
	CURRENT_TIMESTAMP, NEW.executor_id, NULL);
    RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_notify_task_insert
AFTER INSERT ON tasks
FOR EACH ROW
EXECUTE FUNCTION notify_task_insert();

------UPDATE
CREATE OR REPLACE FUNCTION notify_task_update()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.executor_id != OLD.executor_id AND NEW.executor_id IS NOT NULL THEN
        INSERT INTO notifications (message, time, user_id, project_id)
        VALUES ('You have been unassigned from task '||OLD.title,
		CURRENT_TIMESTAMP, OLD.executor_id, NULL);
		
		INSERT INTO notifications (message, time, user_id, project_id)
        VALUES ('You have been assigned to task '||NEW.title,
		CURRENT_TIMESTAMP, NEW.executor_id, NULL);
    END IF;
    RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_notify_task_update
AFTER UPDATE ON tasks
FOR EACH ROW
EXECUTE FUNCTION notify_task_update();

------DELETE
CREATE OR REPLACE FUNCTION notify_task_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO notifications (message, time, user_id, project_id)
	VALUES ('You have been unassigned from task '||OLD.title,
	CURRENT_TIMESTAMP, OLD.executor_id, NULL);
    RETURN OLD;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_notify_task_delete
AFTER DELETE ON tasks
FOR EACH ROW
EXECUTE FUNCTION notify_task_delete();

----TASKSTATUSES
CREATE OR REPLACE FUNCTION notify_status_update()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.status != OLD.status AND NEW.executor_id IS NOT NULL THEN
        INSERT INTO notifications (message, time, user_id, project_id)
        VALUES ('Status of your task '||NEW.title||
		' has been changed to '||(SELECT name FROM taskstatuses WHERE id = NEW.status),
		CURRENT_TIMESTAMP, NEW.executor_id, NULL);
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_notify_status_update
AFTER UPDATE ON tasks
FOR EACH ROW
EXECUTE FUNCTION notify_status_update();

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
	CURRENT_TIMESTAMP, NEW.executor_id);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_task_insert
AFTER INSERT ON tasks
FOR EACH ROW
EXECUTE FUNCTION log_task_insert();

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
	CURRENT_TIMESTAMP, NEW.executor_id);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_task_update
AFTER UPDATE ON tasks
FOR EACH ROW
EXECUTE FUNCTION log_task_update();

------DELETE
CREATE OR REPLACE FUNCTION log_task_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Deleted task with id '||OLD.id||
	' and title '||OLD.title,
	CURRENT_TIMESTAMP, OLD.executor_id);
	RETURN OLD;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_task_delete
AFTER DELETE ON tasks
FOR EACH ROW
EXECUTE FUNCTION log_task_delete();

----PROJECT

------INSERTION
CREATE OR REPLACE FUNCTION log_project_insert()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Created project with id '||NEW.id||
	' and title '||NEW.title,
	CURRENT_TIMESTAMP, NULL);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_project_insert
AFTER INSERT ON projects
FOR EACH ROW
EXECUTE FUNCTION log_project_insert();

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
	CURRENT_TIMESTAMP, NULL);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_project_update
AFTER UPDATE ON projects
FOR EACH ROW
EXECUTE FUNCTION log_project_update();

------DELETE
CREATE OR REPLACE FUNCTION log_project_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Deleted project with id '||OLD.id
	||' and title '||OLD.title,
	CURRENT_TIMESTAMP, NULL);
	RETURN OLD;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_project_delete
AFTER UPDATE ON projects
FOR EACH ROW
EXECUTE FUNCTION log_project_update();

----TASKCOMMENTS

------INSERTION
CREATE OR REPLACE FUNCTION log_taskcomment_insert()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Created taskcomment with id '||NEW.id||
	' by '||(SELECT name FROM users WHERE users.id = NEW.author_id),
	CURRENT_TIMESTAMP, NEW.author_id);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_taskcomment_insert
AFTER INSERT ON taskcomments
FOR EACH ROW
EXECUTE FUNCTION log_taskcomment_insert();

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
	CURRENT_TIMESTAMP, NEW.author_id);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_taskcomment_update
AFTER UPDATE ON taskcomments
FOR EACH ROW
EXECUTE FUNCTION log_taskcomment_update();

------DELETE
CREATE OR REPLACE FUNCTION log_taskcomment_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Deleted taskcomment with id '||OLD.id||
	' by '||(SELECT name FROM users WHERE users.id = OLD.author_id),
	CURRENT_TIMESTAMP, OLD.author_id);
	RETURN OLD;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_taskcomment_delete
AFTER UPDATE ON taskcomments
FOR EACH ROW
EXECUTE FUNCTION log_taskcomment_update();

----USERS

------INSERT
CREATE OR REPLACE FUNCTION log_users_insert()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Registered user account with id '||NEW.id||
	' , name '||NEW.name||
	' , email '||NEW.email,
	CURRENT_TIMESTAMP, NEW.id);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_users_insert
AFTER INSERT ON users
FOR EACH ROW
EXECUTE FUNCTION log_users_insert();

------UPDATE
CREATE OR REPLACE FUNCTION log_users_update()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Updated user account with id '||NEW.id||
	CASE
		WHEN OLD.name != NEW.name THEN
			', changed name from '||OLD.name||
			' to '||NEW.name
			
		WHEN OLD.email != NEW.email THEN
			', changed email from '||OLD.email||
			' to '||NEW.email
			
		ELSE
			', some fields changed'
	END,
	CURRENT_TIMESTAMP, NEW.id);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_users_update
AFTER UPDATE ON users
FOR EACH ROW
EXECUTE FUNCTION log_users_update();

------DELETE
CREATE OR REPLACE FUNCTION log_users_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Deleted user account with id '||OLD.id||
	', name '||OLD.name||
	', email '||OLD.email,
	CURRENT_TIMESTAMP, NULL);
	RETURN OLD;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_users_delete
AFTER DELETE ON users
FOR EACH ROW
EXECUTE FUNCTION log_users_delete();

----USERPROFILES

------INSERT
CREATE OR REPLACE FUNCTION log_userprofiles_insert()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Created userprofile with id '||NEW.user_id||
	' , phone '||NEW.phone||
	' , address '||NEW.address||
	' , date of birth '||NEW.date_of_birth,
	CURRENT_TIMESTAMP, NEW.user_id);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_userprofiles_insert
AFTER INSERT ON userprofiles
FOR EACH ROW
EXECUTE FUNCTION log_userprofiles_insert();

------UPDATE
CREATE OR REPLACE FUNCTION log_userprofiles_update()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Updated userprofile with id '||NEW.user_id||
	CASE
		WHEN OLD.phone != NEW.phone THEN
			', changed phone from '||OLD.phone||
			' to '||NEW.phone
			
		WHEN OLD.address != NEW.address THEN
			', changed address from '||OLD.address||
			' to '||NEW.address
			
		WHEN OLD.date_of_birth != NEW.date_of_birth THEN
			', changed date of birth from '||OLD.date_of_birth||
			' to '||NEW.date_of_birth
			
		ELSE
			', some fields changed'
	END,
	CURRENT_TIMESTAMP, NEW.user_id);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_userprofiles_update
AFTER UPDATE ON userprofiles
FOR EACH ROW
EXECUTE FUNCTION log_userprofiles_update();

------DELETE
CREATE OR REPLACE FUNCTION log_userprofiles_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Deleted userprofile with id '||OLD.user_id||
	' , phone '||OLD.phone||
	' , address '||OLD.address||
	' , date of birth '||OLD.date_of_birth,
	CURRENT_TIMESTAMP, OLD.user_id);
	RETURN OLD;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_userprofiles_delete
AFTER DELETE ON userprofiles
FOR EACH ROW
EXECUTE FUNCTION log_userprofiles_delete();

----PROJECTRESOURCES

------INSERT
CREATE OR REPLACE FUNCTION log_projectresources_insert()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Created project resources with id '||NEW.id||
	' of type '||NEW.type||
	' in project '||(SELECT title FROM projects WHERE id = NEW.project_id),
	CURRENT_TIMESTAMP, NULL);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_projectresources_insert
AFTER INSERT ON projectresources
FOR EACH ROW
EXECUTE FUNCTION log_projectresources_insert();

------UPDATE
CREATE OR REPLACE FUNCTION log_projectresources_update()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Updated project resources with id '||NEW.id||
	CASE
		WHEN OLD.type != NEW.type THEN
			', changed type from '||OLD.type||
			' to '||NEW.type
			
		WHEN OLD.project_id != NEW.project_id THEN
		', changed project from '||(SELECT title FROM projects WHERE id = OLD.project_id)||
		' to '||(SELECT title FROM projects WHERE id = NEW.project_id)
		
		ELSE
			', some fields changed'
	END,
	CURRENT_TIMESTAMP, NULL);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_projectresources_update
AFTER UPDATE ON projectresources
FOR EACH ROW
EXECUTE FUNCTION log_projectresources_update();

------DELETE
CREATE OR REPLACE FUNCTION log_projectresources_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Deleted project resources with id '||OLD.id||
	' of type '||OLD.type||
	' in project '||(SELECT title FROM projects WHERE id = OLD.project_id),
	CURRENT_TIMESTAMP, NULL);
	RETURN OLD;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_projectresources_delete
AFTER DELETE ON projectresources
FOR EACH ROW
EXECUTE FUNCTION log_projectresources_delete();

----USERROLES

------INSERT
CREATE OR REPLACE FUNCTION log_userroles_insert()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Created userroles for user '||(SELECT name FROM users WHERE id = NEW.user_id)||
	' with role '||(SELECT name FROM roles WHERE id = NEW.role_id)||
	' in project '||(SELECT title FROM projects WHERE id = NEW.project_id),
	CURRENT_TIMESTAMP, NULL);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_userroles_insert
AFTER INSERT ON userroles
FOR EACH ROW
EXECUTE FUNCTION log_userroles_insert();

------UPDATE
CREATE OR REPLACE FUNCTION log_userroles_update()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Updated userroles for user '||(SELECT name FROM users WHERE id = NEW.user_id)||
	' to role '||(SELECT name FROM roles WHERE id = NEW.role_id)||
	' in project '||(SELECT title FROM projects WHERE id = NEW.project_id),
	CURRENT_TIMESTAMP, NULL);
	RETURN NEW;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_log_userroles_update
AFTER UPDATE ON userroles
FOR EACH ROW
EXECUTE FUNCTION log_userroles_update();

------DELETE
CREATE OR REPLACE FUNCTION log_userroles_delete()
RETURNS TRIGGER AS $$
BEGIN
	INSERT INTO logs (action, date, user_id)
	VALUES ('Deleted userroles for user '||(SELECT name FROM users WHERE id = OLD.user_id)||
	' with role '||(SELECT name FROM roles WHERE id = OLD.role_id)||
	' in project '||(SELECT title FROM projects WHERE id = OLD.project_id),
	CURRENT_TIMESTAMP, NULL);
	RETURN OLD;
END; $$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_userroles_delete
AFTER DELETE ON userroles
FOR EACH ROW
EXECUTE FUNCTION log_userroles_delete();