create table category_product
(
	id int generated always as identity primary key,
	_name varchar
);

create table products 
(
	id int generated always as identity primary key,
	_name varchar not null,
	_price numeric not null,
	category_id int references category_product(id) ON DELETE CASCADE
);

create table providers 
(
	id int generated always as identity primary key,
	_name varchar not null,
	_phone_number bigint not null
);

create table products_providers
(
	id int generated always as identity primary key,
	products_id int references products(id) ON DELETE CASCADE,
	providers_id int references providers(id) ON DELETE CASCADE
);

insert into category_product (_name)
values ('household'),('dairy'),('alcohol');

insert into products(_name, _price, category_id)
values ('milk', 56.34, 2), ('soap', 50.05, 1), ('beer', 68.75, 3);

insert into providers(_name, _phone_number)
values ('MainWall Company', 238912), ('AlcoBro Company', 902301);

insert into products_providers (products_id, providers_id)
values (1,1),(2,1),(3,2);

