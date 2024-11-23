# Integrantes

- João Pedro Moura Tuneli **RM:93530**

- Enzo Massayuki Obayashi **RM:95634**

## Evidencias de Funcionamento

### Evidencias de erro(404,401)

- **GET**

![text](evidencias/status404get.jpeg)

- **POST**

![text](evidencias/status400post.jpeg)

### Requisições funcionando

- **GET**

![text](evidencias/status200get.jpeg)

- **POST**

![text](evidencias/status201post.jpeg)

- **Serviço Operacional**(Health)

![text](evidencias/serviçoemfuncionamento.jpeg)

----

# Integração com MongoDB

![text](evidencias/conexao.jpeg)

# Requisição Sem e com Redis , para teste de desenpenho:

## Sem Redis

- **1° Requisição(13ms)**

![alt text](evidencias/1comredis.png)

- **2° Requisição(29ms)**

![alt text](evidencias/2comredis.png)

- **3° Requisição(9ms)**

![alt text](evidencias/3comredis.png)

## Com Redis

- **1° Requisição(18ms)**

![alt text](evidencias/3semredis.png)

- **2° Requisição(26ms)**

![alt text](evidencias/3semredis.png)

- **3° Requisição(9ms)**

![alt text](evidencias/3semredis.png)

## Comparação de Desempenho: Com Redis x Sem Redis

| Condição      | Requisição 1 (ms) | Requisição 2 (ms) | Requisição 3 (ms) | Tempo Médio (ms) |
|---------------|-------------------|-------------------|-------------------|------------------|
| **Sem Redis** | 13                | 29                | 9                 | 17               |
| **Com Redis** | 18                | 26                | 9                 | 17.67            |

----

# Testes com XUnit:

## Testes realizados:

- [ ] Inserção de dados no MongoDB
- [ ] Recuperação de dados do cache Redis
- [ ] Respostas corretas de Status Codes
- [ ] Cenário de erro ao consultar MongoDB
- [ ] Cenário de erro ao acessar o Redis

## Resultado:

![alt text](evidencias/testeRedis.png)