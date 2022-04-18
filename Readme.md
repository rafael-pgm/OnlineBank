# Projeto OnlineBank

Trata-se de um projeto em que crio a estrutura backend de um banco virtual tendo por foco estabelecer uma conexão com um banco de dados relacional local por meio de uma connection string fornecida pelo SQL Server, possibilitando a criação de contas, a listagem das contas, a atualização e a exclusão de uma conta presente no banco de dados por meio do programa.

Para o programa funcionar corretamente é necessário inserir a connection string nos respectivos campos em UserDataConnection.cs, UserAccountConnection.cs e em TransferConnection.cs. 

Para a criação da Database e da tabela utilizada junto com o programa, seguem os comandos em SQL:


CREATE DATABASE OnlineBank;

CREATE TABLE clients(
	ID INT NOT NULL 
	,Name VARCHAR(200) NOT NULL  
	,Agency VARCHAR(4) NOT NULL 
	,AccountNumber VARCHAR(5) NOT NULL 
	,Balance BIGINT DEFAULT 0
	,Credit BIGINT DEFAULT 0
	,Email VARCHAR(100) NOT NULL 
	,Adress VARCHAR (100) NOT NULL 
	,Phone VARCHAR(11) NOT NULL 
	,MaxCredit BIGINT DEFAULT 0
);