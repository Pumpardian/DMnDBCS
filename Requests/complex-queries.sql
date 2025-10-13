--Get completed projects
SELECT
	title,
	description,
	start_date,
	end_date
FROM projects
WHERE start_date IS NOT NULL AND end_date IS NOT NULL;
	
--Get completed projects with task count
SELECT
	title,
	description,
	start_date,
	end_date,
	(
		SELECT COUNT(*)
		FROM tasks
		WHERE tasks.project_id = projects.id
	) AS task_count
FROM projects
WHERE
	start_date IS NOT NULL
	AND end_date IS NOT NULL
ORDER BY title;

--Get tasks with their executors
SELECT DISTINCT
	users.name,
	users.email,
	tasks.title AS task_title
FROM users
	INNER JOIN tasks on users.id = tasks.executor_id;
	
--Get uncompleted tasks with their executors
SELECT DISTINCT
	users.name,
	users.email,
	tasks.title AS task_title
FROM users
	INNER JOIN tasks on users.id = tasks.executor_id
WHERE EXISTS
	(
		SELECT 1
		FROM taskstatuses
		WHERE
			taskstatuses.id = tasks.status
			AND taskstatuses.name != 'Completed'
	);
	
--Get projects with their tasks
SELECT
	projects.title,
	tasks.title AS task_title
FROM projects
	LEFT JOIN tasks ON projects.id = tasks.project_id
ORDER BY projects.title, tasks.title;
	
--Get all tasks and all projetcs
SELECT
	projects.title,
	tasks.title AS task_title
FROM projects
	FULL OUTER JOIN tasks ON projects.id = tasks.project_id
ORDER BY projects.title, tasks.title;
	
--Get user info
SELECT
	users.name,
	users.email,
	userprofiles.phone,
	userprofiles.address,
	userprofiles.date_of_birth,
	userprofiles.profile_picture
FROM users
	LEFT JOIN userprofiles ON users.id = userprofiles.user_id
ORDER BY users.name;

--Get user's projects & role in them
SELECT
	users.name,
	users.email,
	projects.title,
	roles.name
FROM users
	LEFT JOIN userroles ON users.id = userroles.user_id
	INNER JOIN projects ON userroles.project_id = projects.id
	INNER JOIN roles ON userroles.role_id = roles.id
ORDER BY users.name;

--Get user's project count
SELECT DISTINCT
	users.name,
	users.email,
	COUNT(projects.id)
FROM users
	LEFT JOIN userroles ON users.id = userroles.user_id
	INNER JOIN projects ON userroles.project_id = projects.id
GROUP BY users.email, users.name
ORDER BY users.name;

--ABOVE WITH SUBQUERY
SELECT DISTINCT
	users.name,
	users.email,
	(
		SELECT COUNT(*)
		FROM projects
			INNER JOIN userroles ON projects.id = userroles.project_id
		WHERE
			userroles.user_id = users.id
	)
FROM users
ORDER BY users.name;

--Get tasks	with planned & in progress statuses
SELECT
	tasks.title,
	tasks.description,
	tasks.creation_date,
	taskstatuses.name
FROM tasks
	LEFT JOIN taskstatuses ON tasks.status = taskstatuses.id
WHERE
	taskstatuses.name = 'In progress'
	OR taskstatuses.name = 'Planned';
	
--ABOVE QUERY USING UNION
SELECT
	tasks.title,
	tasks.description,
	tasks.creation_date,
	taskstatuses.name
FROM tasks
	LEFT JOIN taskstatuses ON tasks.status = taskstatuses.id
WHERE
	taskstatuses.name = 'In progress'
UNION
SELECT
	tasks.title,
	tasks.description,
	tasks.creation_date,
	taskstatuses.name
FROM tasks
	LEFT JOIN taskstatuses ON tasks.status = taskstatuses.id
WHERE
	taskstatuses.name = 'Planned';
	
--Get projects with assigned non-completed tasks
SELECT projects.title
FROM projects
WHERE EXISTS
	(
        SELECT 1
        FROM tasks
			LEFT JOIN taskstatuses ON tasks.status = taskstatuses.id
        WHERE tasks.project_id = projects.id
            AND taskstatuses.name != 'Completed'
    );

--Get projects size based on task count
SELECT
	projects.title,
	CASE
		WHEN COUNT(tasks.id) < 3 THEN 'Small'
		WHEN COUNT(tasks.id) < 10 THEN 'Medium'
		ELSE 'Large'
	END as project_size
FROM projects
	LEFT JOIN tasks ON projects.id = tasks.project_id
GROUP BY projects.title
ORDER BY projects.title;

--ABOVE USING WITH
WITH project_stats AS
(
	SELECT
		projects.title,
		COUNT(tasks.id) as task_count
	FROM projects
		LEFT JOIN tasks ON projects.id = tasks.project_id
	GROUP BY projects.title
)
SELECT
    title,
    CASE
        WHEN task_count < 3 THEN 'Small'
        WHEN task_count < 10 THEN 'Medium'
        ELSE 'Large'
    END as project_size
FROM project_stats
ORDER BY title;

--Get completed project tasks query plan
EXPLAIN
SELECT *
FROM tasks
	LEFT JOIN taskstatuses ON tasks.status = taskstatuses.id
WHERE taskstatuses.name = 'Completed';

--Get top 3 project contributors (RANK - with repetion & skip, DENSE_RANK - with repetion, ROW_NUMBER - without)
SELECT *
FROM
(
	SELECT
		users.name,
		tasks.project_id,
		COUNT(*) AS tasks_completed,
		RANK() OVER
		(
			PARTITION BY tasks.project_id
			ORDER BY COUNT(*) DESC
		) AS rank
	FROM users
		LEFT JOIN tasks ON users.id = tasks.executor_id
		INNER JOIN taskstatuses ON tasks.status = taskstatuses.id
	WHERE
		taskstatuses.name = 'Completed'
	GROUP BY users.name, tasks.project_id
) stats
WHERE
	rank <= 3;
	
--ABOVE USING WITH
WITH ranked_tasks AS
(
    SELECT
		users.name,
		tasks.project_id,
        COUNT(*) AS tasks_completed,
        RANK() OVER (
            PARTITION BY tasks.project_id
            ORDER BY COUNT(*) DESC
        ) AS place
    FROM users
        LEFT JOIN tasks ON users.id = tasks.executor_id
        INNER JOIN taskstatuses ON tasks.status = taskstatuses.id
    WHERE taskstatuses.name = 'Completed'
    GROUP BY users.name, tasks.project_id
)
SELECT *
FROM ranked_tasks
WHERE
	place <= 3;