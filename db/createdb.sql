
CREATE TABLE customers (
    customer_id UUID PRIMARY KEY NOT NULL,
    first_name VARCHAR(255) NOT NULL,
    middle_name VARCHAR(255) NOT NULL,
    date_of_birth TIMESTAMP WITH TIME ZONE NOT NULL,
    cpf VARCHAR(20) NOT NULL
);
CREATE TABLE customer_addresses(
    address_id UUID PRIMARY KEY NOT NULL,
    customer_id UUID NOT NULL REFERENCES customers(customer_id) ON DELETE CASCADE,
    address_line_1 VARCHAR(255) NOT NULL,
    address_line_2 VARCHAR(255),
    number INT NOT NULL,
    district VARCHAR(100),
    city VARCHAR(100) NOT NULL,
    state VARCHAR(100) NOT NULL,
    postal_code VARCHAR(20) NOT NULL
);