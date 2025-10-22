-- Script de criação do banco de dados PostgreSQL para MauiApp1
-- Execute este script no PostgreSQL para criar as tabelas necessárias

-- Criar banco de dados (execute como superuser)
-- CREATE DATABASE mauiapp1;
-- CREATE DATABASE mauiapp1_dev;

-- Conectar ao banco mauiapp1 e executar os comandos abaixo:

-- Tabela de Insumos
CREATE TABLE IF NOT EXISTS Insumos (
    Id SERIAL PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Descricao TEXT,
    Preco DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    Quantidade INTEGER NOT NULL DEFAULT 0,
    Unidade VARCHAR(50) NOT NULL DEFAULT 'UN',
    DataCriacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DataAtualizacao TIMESTAMP NULL,
    Ativo BOOLEAN NOT NULL DEFAULT TRUE
);

-- Tabela de Serviços
CREATE TABLE IF NOT EXISTS Servicos (
    Id SERIAL PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Descricao TEXT,
    Preco DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    DuracaoMinutos INTEGER NOT NULL DEFAULT 60,
    Categoria VARCHAR(100) NOT NULL DEFAULT 'Geral',
    DataCriacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DataAtualizacao TIMESTAMP NULL,
    Ativo BOOLEAN NOT NULL DEFAULT TRUE
);

-- Índices para melhorar performance
CREATE INDEX IF NOT EXISTS idx_insumos_nome ON Insumos(Nome);
CREATE INDEX IF NOT EXISTS idx_insumos_ativo ON Insumos(Ativo);
CREATE INDEX IF NOT EXISTS idx_insumos_data_criacao ON Insumos(DataCriacao);

CREATE INDEX IF NOT EXISTS idx_servicos_nome ON Servicos(Nome);
CREATE INDEX IF NOT EXISTS idx_servicos_categoria ON Servicos(Categoria);
CREATE INDEX IF NOT EXISTS idx_servicos_ativo ON Servicos(Ativo);
CREATE INDEX IF NOT EXISTS idx_servicos_data_criacao ON Servicos(DataCriacao);

-- Dados de exemplo (opcional)
INSERT INTO Insumos (Nome, Descricao, Preco, Quantidade, Unidade) VALUES
('Parafuso M6x20', 'Parafuso de aço inox 6mm x 20mm', 0.50, 1000, 'UN'),
('Tinta Branca', 'Tinta acrílica branca 1L', 25.00, 50, 'L'),
('Fio Elétrico 2.5mm', 'Fio elétrico 2.5mm²', 3.50, 500, 'M'),
('Lâmpada LED 9W', 'Lâmpada LED 9W E27', 15.00, 200, 'UN'),
('Cabo de Rede CAT6', 'Cabo de rede categoria 6', 2.00, 1000, 'M');

INSERT INTO Servicos (Nome, Descricao, Preco, DuracaoMinutos, Categoria) VALUES
('Instalação Elétrica', 'Instalação de sistema elétrico residencial', 150.00, 480, 'Elétrica'),
('Pintura Interna', 'Pintura de paredes internas', 80.00, 240, 'Pintura'),
('Montagem de Móveis', 'Montagem de móveis planejados', 120.00, 180, 'Montagem'),
('Manutenção Hidráulica', 'Reparo e manutenção hidráulica', 100.00, 120, 'Hidráulica'),
('Instalação de Ar Condicionado', 'Instalação de sistema de ar condicionado', 200.00, 300, 'Climatização');