# Documentação do Projeto - Sistema de Feedback para a Plataforma IQuirium

## Resumo do Projeto
Este projeto foi desenvolvido por cinco alunos do curso de Sistemas de Informação da Faculdade IBMEC, em parceria com a empresa IQuirium. O objetivo do projeto é criar uma solução para capturar e gerenciar feedbacks dos usuários da plataforma IQuirium, tanto sobre o produto quanto entre os usuários, visando promover o desenvolvimento contínuo da plataforma e o autodesenvolvimento dos usuários.

## Equipe do Projeto
- **Alunos**:João Victor Campelo, Thiago Novaes, Bernardo Pinto, Daniel Lloyd, Marceu Filho
- **Curso**: Ciência de Dados e Engenharia de software 
- **Instituição**: IBMEC
- **Parceria**: IQuirium

## Visão Geral do Sistema

### 1. Feedbacks do Produto
Esta funcionalidade permite que os usuários forneçam feedback diretamente sobre o produto, incluindo sugestões de melhoria, comentários gerais ou relatos de erros técnicos. Esses feedbacks serão armazenados para análise futura pela equipe da plataforma, ajudando a identificar áreas de melhoria e novas funcionalidades a serem desenvolvidas.

#### Objetivo
A captura de feedback dos usuários visa otimizar a experiência de uso da plataforma, identificando áreas de melhoria com base na experiência real dos usuários.

#### Funcionalidades
- Envio de feedback espontâneo diretamente na plataforma.
- Feedbacks associados ao perfil do usuário, permitindo análises mais detalhadas pela equipe de desenvolvimento.
- Interface de formulário intuitiva e simples.
- Feedback ilimitado: os usuários podem enviar quantos feedbacks desejarem.

#### Requisitos Técnicos
- **Banco de Dados**: Armazenar os feedbacks com informações como ID do usuário, texto do feedback, categoria, data e hora.
- **Frontend**: Formulário acessível na plataforma, com confirmação de envio.
- **Backend**: APIs para receber e armazenar feedbacks de forma segura.
- **Autenticação**: Apenas usuários autenticados podem enviar feedback.

#### Métricas de Sucesso
- Número total de feedbacks trocados.
- Taxa de resposta a solicitações de feedback.
- Engajamento dos usuários medido pela recorrência.

### Diagrama de Classe - Produto

![image](https://github.com/user-attachments/assets/cac149e4-dba0-41a7-b92e-fc5569918a37)

### 2. Feedbacks entre Usuários
Esta funcionalidade permite a troca de feedbacks entre os usuários da plataforma IQuirium, promovendo uma cultura de colaboração e desenvolvimento pessoal. Os usuários podem solicitar feedback de outros ou enviá-lo espontaneamente.

#### Objetivo
Facilitar a troca de avaliações construtivas entre os usuários, promovendo o crescimento contínuo de competências e comportamentos.

#### Funcionalidades
- Solicitação de feedback a outros usuários, com a possibilidade de incluir um contexto.
- Envio espontâneo de feedbacks.
- Feedbacks privados, visíveis apenas pelo remetente e destinatário.
- Relatório de feedbacks inapropriados, com revisão pela equipe de moderação.

#### Requisitos Técnicos
- **Banco de Dados**: Armazenar feedbacks trocados entre os usuários e logs de reports de feedbacks.
- **Frontend**: Interface para envio e recebimento de feedbacks, com opção de reportar conteúdos ofensivos.
- **Backend**: APIs para gerenciamento de feedbacks e moderação de conteúdo reportado.
- **Autenticação e Autorização**: Apenas usuários autenticados podem trocar feedbacks.

#### Métricas de Sucesso
- Número total de feedbacks trocados.
- Taxa de resposta a solicitações de feedback.
- Engajamento dos usuários medido pela recorrência.

### Diagrama de Classe - Usuario 
![image](https://github.com/user-attachments/assets/8bc955e8-c9ef-4b66-93ce-585b1129a929)

---

## Requisitos Funcionais

1. **Cadastro de Usuários**
   - O sistema deve permitir que novos usuários se cadastrem fornecendo nome, email, e senha.
   
2. **Autenticação de Usuários**
   - O sistema deve permitir que usuários façam login com suas credenciais.
   
3. **Envio de Feedback do Produto**
   - O sistema deve permitir que usuários autenticados enviem feedbacks sobre o produto, classificando-os em categorias como sugestões, problemas, ou melhorias.

4. **Troca de Feedback Entre Usuários**
   - O sistema deve permitir que os usuários enviem e recebam feedbacks entre si.

5. **Moderação de Feedbacks**
   - O sistema deve fornecer uma funcionalidade para que usuários reportem feedbacks inapropriados, e esses relatórios devem ser revisados pela equipe de moderação.

6. **Painel de Administração**
   - O sistema deve ter um painel para que administradores possam visualizar e gerenciar feedbacks recebidos e reportados.

7. **Relatórios de Feedback**
   - O sistema deve permitir que os usuários visualizem um histórico de feedbacks trocados e enviados.

---
## Diagrama de caso de uso
![image](https://github.com/user-attachments/assets/9cc2ea8c-df74-4e91-b946-889b89897718)
