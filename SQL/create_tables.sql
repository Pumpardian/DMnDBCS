-- Таблицы с ограничениями
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS userprofiles (
    user_id SERIAL PRIMARY KEY REFERENCES users(id),
    phone VARCHAR(20) NOT NULL UNIQUE,
    address VARCHAR(255) NOT NULL,
    date_of_birth DATE NOT NULL,
    profile_picture TEXT,
    CONSTRAINT check_phone CHECK (phone ~ '^\+375\d{9}$')
);

CREATE TABLE IF NOT EXISTS taskstatuses (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS projects (
    id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    start_date DATE NOT NULL,
    end_date DATE,
    CONSTRAINT check_dates CHECK (
        end_date IS NULL
        OR end_date >= start_date
    )
);

CREATE TABLE IF NOT EXISTS tasks (
    id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    status SERIAL REFERENCES taskstatuses(id),
    creation_date DATE NOT NULL,
    completion_date DATE,
    project_id SERIAL REFERENCES projects(id),
    executor_id SERIAL REFERENCES users(id)
);

CREATE TABLE IF NOT EXISTS taskcomments (
    id SERIAL PRIMARY KEY,
    content TEXT NOT NULL,
    creation_date DATE NOT NULL,
    task_id SERIAL REFERENCES tasks(id),
    author_id SERIAL REFERENCES users(id)
);

CREATE TABLE IF NOT EXISTS userroles (
    user_id SERIAL REFERENCES users(id),
    role_id SERIAL REFERENCES roles(id),
    project_id SERIAL REFERENCES projects(id),
    PRIMARY KEY (user_id, project_id, role_id)
);

CREATE TABLE IF NOT EXISTS projectresources (
    id SERIAL PRIMARY KEY,
    description TEXT,
    type VARCHAR(50) NOT NULL,
    project_id SERIAL REFERENCES projects(id)
);

CREATE TABLE IF NOT EXISTS logs (
    id SERIAL PRIMARY KEY,
    action TEXT NOT NULL,
    date TIMESTAMP NOT NULL,
    user_id SERIAL REFERENCES users(id)
);

CREATE TABLE IF NOT EXISTS notifications (
    id SERIAL PRIMARY KEY,
    message TEXT NOT NULL,
    time TIMESTAMP NOT NULL,
    user_id SERIAL REFERENCES users(id),
    project_id SERIAL REFERENCES projects(id)
);

-- Ускорение поиска за счет индексации
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_tasks_creation_date ON tasks(creation_date);
CREATE INDEX idx_taskcomments_creation_date ON taskcomments(creation_date);
CREATE INDEX idx_projectresources_type ON projectresources(type);