<div align="center">
  <h1>🛒💻 TechStore</h1>
  <p><strong>Engenheira de Software Daliene Roque</strong></p>

  <img src="./imagens/banner-techstore.png" width="100%">
</div>

<hr />

<p>
  <strong>TechStore</strong> é uma aplicação completa de <strong>e-commerce</strong>
  desenvolvida como projeto educacional e profissional, simulando um ambiente real de aplicação corporativa. 
  O projeto integra um <strong>backend robusto em ASP.NET Core</strong>
  com um <strong>frontend em Blazor WebAssembly</strong>, aplicando boas práticas de
  arquitetura e organização de código.
</p>
<p>
  Este projeto foi desenvolvido com apoio de ferramentas como IA (ChatGPT),
  sendo utilizado como um acelerador de aprendizado. Todo o código foi estudado,
  adaptado e compreendido durante o desenvolvimento.
</p>

<hr />

<h2>🌐 Deploy</h2>

 <p align="center">
  <a href="https://lojatechstore.netlify.app/" target="_blank">
    <img src="https://img.shields.io/badge/Acessar%20Projeto-Online-28a745?style=for-the-badge&logo=vercel&logoColor=white">
  </a>
</p>

<p>
  A aplicação está totalmente publicada e acessível online:
</p>

<ul>
  <li><strong>Frontend:</strong> Blazor WebAssembly hospedado no Netlify</li>
  <li><strong>Backend:</strong> API em ASP.NET Core hospedada no Render</li>
  <li><strong>Banco de Dados:</strong> PostgreSQL em ambiente cloud via Render</li>
</ul>

<hr />

<h2>📌 Visão Geral</h2>

<p>A aplicação permite que o usuário:</p>

<ul>
  <li>Crie uma conta e realize login</li>
  <li>Navegue pelos produtos</li>
  <li>Pesquise produtos pelo nome</li>
  <li>Adicione produtos ao carrinho</li>
  <li>Visualize o carrinho com cálculo automático do total</li>
  <li>Finalize a compra com confirmação do pedido</li>
</ul>

<hr />

<h2>🚀 Funcionalidades</h2>

<h3>🔐 Autenticação</h3>
<ul>
  <li>Registro de usuários</li>
  <li>Login com autenticação JWT</li>
  <li>Controle de acesso</li>
</ul>

<h3>🛍️ Produtos</h3>
<ul>
  <li>Listagem de produtos</li>
  <li>Busca de produtos por nome</li>
  <li>Controle de estoque</li>
  <li>Relacionamento Categoria ↔ Produtos</li>
  <li>Exclusão lógica (Soft Delete)</li>
</ul>

<h3>🛒 Carrinho de Compras</h3>
<ul>
  <li>Adicionar produtos ao carrinho</li>
  <li>Remover produtos</li>
  <li>Cálculo automático do valor total</li>
  <li>Carrinho vinculado ao usuário autenticado</li>
</ul>

<h3>📦 Pedidos</h3>
<ul>
  <li>Criação de pedidos a partir do carrinho</li>
  <li>Integração com PedidosController</li>
  <li>Limpeza automática do carrinho após a compra</li>
  <li>Tela de confirmação do pedido</li>
</ul>

<h3>🛠️ Painel de Administrador</h3>
<ul>
  <li>Acesso restrito por Role (Admin)</li>
  <li>Cadastro de novos produtos</li>
  <li>Edição de nome, descrição, preço, estoque e imagem</li>
  <li>Exclusão de produtos</li>
  <li>Integração total com API via DTOs específicos</li>
  <li>Pré-visualização dinâmica da imagem por URL</li>
</ul>

<hr />

<h2>📐 Arquitetura</h2>

<pre>
TechStore
│
├── API            → Controllers e endpoints
├── Application    → Regras de negócio e DTOs
├── Core           → Entidades e Interfaces de repositório
├── Infrastructure → Responsável pela persistência de dados, Contexto de banco de dados e Migrations.
├── Web            → Frontend Blazor WebAssembly
└── Tests          → Projeto preparado para testes, reforçando a importância da qualidade e confiabilidade do código.
</pre>

<p><strong>Padrões utilizados:</strong></p>
<ul>
  <li>Repository Pattern</li>
  <li>DTO Pattern</li>
  <li>Dependency Injection</li>
  <li>Princípios SOLID</li>
  <li>Separação de responsabilidades</li>
</ul>

<hr />

<h2>🛠️ Tecnologias</h2>

<h3>Backend</h3>
<ul>
  <li>C# / .NET 8</li>
  <li>ASP.NET Core Web API</li>
  <li>Entity Framework Core</li>
  <li><strong>PostgreSQL</strong> 🐘 (migração realizada a partir do SQL Server)</li>
  <li>AutoMapper</li>
  <li>Swagger / OpenAPI</li>
</ul>

<h3>Frontend</h3>
<ul>
  <li>Blazor WebAssembly</li>
  <li>HTML</li>
  <li>CSS</li>
  <li>Bootstrap</li>
</ul>

<h3>Geral</h3>
<ul>
  <li>Git & GitHub</li>
  <li>REST API</li>
</ul>

<hr />

<h2>🧠 Aprendizados</h2>

<ul>
  <li>Desenvolvimento de APIs REST com ASP.NET Core</li>
  <li>Estruturação de arquitetura em camadas</li>
  <li>Integração entre frontend Blazor e backend</li>
  <li>Uso de Entity Framework Core com PostgreSQL</li>
  <li>Implementação de autenticação com JWT</li>
  <li>Deploy de aplicações em ambientes cloud (Render e Netlify)</li>
</ul>

<hr />

<h2>📌 Status do Projeto</h2>

<ul>
  <li>✅ Backend funcional</li>
  <li>✅ Frontend em Blazor</li>
  <li>✅ Autenticação implementada</li>
  <li>✅ Carrinho de compras</li>
  <li>✅ Busca de produtos</li>
  <li>✅ Painel Administrador completo</li>
  <li>🔄 Migração de banco realizada (SQL Server → PostgreSQL)</li>
  <li>🚧 Melhorias contínuas</li>
</ul>

<hr />

<h2>🔜 Próximos Passos</h2>

<ul>
   <li>
    Melhorias contínuas na <strong>experiência do usuário (UI/UX)</strong>
    e organização visual da aplicação.
  </li>  
  <li>Testes unitários</li>
</ul>

<hr />

<h2>✉️ Contato</h2>

[![LinkedIn](https://custom-icon-badges.demolab.com/badge/LinkedIn-0A66C2?logo=linkedin-white&logoColor=fff)](https://www.linkedin.com/in/daliene-roque-a5b167269/)
[![GitHub](https://img.shields.io/badge/GitHub-%23121011.svg?logo=github&logoColor=white)](https://github.com/DalieneRoque)
[![YouTube](https://img.shields.io/badge/YouTube-%23FF0000.svg?logo=YouTube&logoColor=white)](https://www.youtube.com/channel/UCzS1CS4ll7-4kWyIwYVhz9w)
[![Discord](https://img.shields.io/badge/Discord-%235865F2.svg?&logo=discord&logoColor=white)](https://discord.gg/5EsYDnNDky)
[![Instagram](https://img.shields.io/badge/Instagram-%23E4405F.svg?logo=Instagram&logoColor=white)](https://www.instagram.com/dalieneroque/)
[![Linktree](https://img.shields.io/badge/LinkTree-1de9b6?logo=linktree&logoColor=white)](https://linktr.ee/dalieneroque)

<hr />

<p>
  ✨ Este projeto representa minha evolução prática em
  <strong>Engenharia de Software</strong>, integrando backend,
  frontend e arquitetura em uma aplicação real.
</p>
