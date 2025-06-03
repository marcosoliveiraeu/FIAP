Esse é o projeto para a fase do tech challenge da pós graduação da fiap , arquitetura .net.

FiapCloudGames - Principais Características

O FiapCloudGames é uma API desenvolvida em .NET 8 para gestão de uma loja de jogos digitais, com foco em organização, segurança e extensibilidade. Suas principais características incluem:
•	Gestão de Jogos: Cadastro, atualização, listagem, consulta e remoção de jogos, com validações detalhadas para título, categoria, preço e data de lançamento.
•	Gestão de Promoções: Criação e administração de promoções, incluindo controle de status, percentual de desconto, datas de validade e associação de jogos às promoções.
•	Gestão de Usuários: Cadastro, atualização, listagem, consulta e remoção de usuários, com controle de perfis (usuário e administrador) e validação de dados sensíveis como e-mail e senha.
•	Associação Usuário-Jogo: Controle dos jogos adquiridos por cada usuário, incluindo histórico de aquisição e valores pagos.
•	Segurança e Autorização: Endpoints protegidos por autenticação e autorização baseada em perfis, garantindo que apenas administradores possam realizar operações sensíveis.
•	Validações Rigorosas: Utilização extensiva de DataAnnotations para garantir integridade e consistência dos dados.
•	Estrutura Modular: Separação clara entre entidades de domínio, DTOs, serviços e controllers, facilitando manutenção e evolução do sistema.
•	Documentação e Boas Práticas: Código comentado, seguindo boas práticas de desenvolvimento, facilitando o entendimento e a colaboração.

Informações Técnicas
•	Autenticação: Utiliza token JWT para autenticação e autorização dos endpoints.
•	Banco de Dados: Utiliza SQLite como banco de dados relacional.
•	Usuários de Teste: O projeto já possui 2 usuários de teste. Os dados de login (usuário e senha) estão disponíveis no arquivo FiapCloudGames.http na raiz do projeto.

A API está pronta para ser integrada a sistemas web, mobile ou outras soluções que necessitem de uma base robusta para gerenciamento de jogos digitais e promoções.
