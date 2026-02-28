<h1>🛒💻 TechStore</h1>

<p>
  <strong>TechStore</strong> é uma aplicação completa de <strong>e-commerce</strong>
  desenvolvida como projeto educacional e profissional, simulando um cenário real
  de empresa. O projeto integra um <strong>backend robusto em ASP.NET Core</strong>
  com um <strong>frontend em Blazor WebAssembly</strong>, aplicando boas práticas de
  arquitetura e organização de código.
</p>

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
  <li>SQL Server</li>
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

<h2>📌 Status do Projeto</h2>

<ul>
  <li>✅ Backend funcional</li>
  <li>✅ Frontend em Blazor</li>
  <li>✅ Autenticação implementada</li>
  <li>✅ Carrinho de compras</li>
  <li>✅ Busca de produtos</li>
  <li>✅ Painel Administrador completo</li>
  <li>🚧 Melhorias contínuas</li>
</ul>

<hr />

<h2>🔜 Próximos Passos</h2>

<ul>
  <li>
    Upload real de imagens para produtos
  </li>
  <li>
    Melhorias contínuas na <strong>experiência do usuário (UI/UX)</strong>
    e organização visual da aplicação.
  </li>  
  <li>Testes unitários</li>
</ul>

<hr />

<h2>✉️ Contato</h2>

<ul>
  <li>
    GitHub:
    <a href="https://github.com/DalieneRoque" target="_blank">
      DalieneRoque
    </a>
  </li>
  <li>
    LinkedIn:
    <a href="https://www.linkedin.com/in/daliene-nonato-lima-roque-a5b167269/" target="_blank">
      Daliene Nonato Lima Roque
    </a>
  </li>
</ul>

<p>
  ✨ Este projeto representa minha evolução prática em
  <strong>Engenharia de Software</strong>, integrando backend,
  frontend e arquitetura em uma aplicação real.
</p>
