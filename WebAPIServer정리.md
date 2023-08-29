# 목차
0. [게임에서의 웹서버](#게임에서의-웹서버)
2. 이론
   1. [C#](#C)
      - [컨테이너(컬렉션)](#컨테이너컬렉션)
      - [Async, Await](#Async-Await)
   2. [.Net Core](#Net-Core)
      - [Program](#Program)
      - [미들웨어](#미들웨어)
      - [라우팅](#라우팅)
      - [의존성 주입](#의존성-주입Dependency-Injection)
   3. [Redis](#Redis)
      - [Redis의 특징](#Redis의-특징)
      - [Redis 명령어](#Redis-명령어)
      - [CloudStructures](#CloudStructures)
   4. [HTTP](#HTTP)
      - [대표적인 HTTP 메소드](#대표적인-HTTP-메소드)
3. [프로그래밍](#프로그래밍)
   1. Program.cs(#Program.cs)
   2. [컨트롤러 공통](#컨트롤러-공통)
   3. [계정 생성](#계정-생성)
   4. [로그인 및 게임 데이터 로딩](#로그인-및-게임-데이터-로딩)
      - [로그인](#로그인)
      - [인증토큰 등록](#인증토큰-등록)
      - [게임 데이터 로딩](#게임-데이터-로딩)
   5. [우편함](#우편함)
      - [우편함 로딩](#우편함-로딩)
      - [메일 전송](#메일-전송)
      - [메일 받기](#메일-받기)
      - [모든 메일 받기](#모든-메일-받기)
      - [받은 메일 삭제](#받은-메일-삭제)
      - [안읽은 메일만 보기](#안읽은-메일만-보기)
      - [메일 정렬](#메일-정렬)
      - [우편함 업데이트](#우편함-업데이트)
      - [아이템 등록](#아이템-등록)
   6. [유저 인증](#유저-인증)
   7. [Redis를 활용한 Lock](#Redis를-활용한-Lock)

---
# 게임에서의 웹서버
## 게임에서 웹서버를 사용하는 이유
   1. 개발의 편의성 및 개발속도(자료와 라이브러리가 많기 때문에)
   2. 서버에 오류가 발생하였을때, 프로그램(유저) 전체에 문제가 생기는 것이 아닌 일부에만 문제가 발생
   3. 모든 데이터는 DB를 이용하여 처리하기 때문에 서버 다운 시 롤백이 없음
   4. Stateless이기 때문에 서버 확장이 비교적 쉽다

## 웹서버를 활용한 서버 구조
![image](https://github.com/Kimseongtae9911/WebAPIServer/assets/84197808/0fa67053-e77d-4f67-b400-3f0e56759ba2)
````
Load Balancer는 클라이언트의 요청을 서버로 균일하게 전달하는 역할을 한다
클라이언트들의 요청은 Load Balancer를 거쳐 서버에 분배되고
각 서버는 요청에 따라 로직을 수행한다
````

---
# C#
    - 마이크로소프트에서 개발한 .Net Framewor 기반 범용 목적의 다중 패러다임 프로그래밍 언어

## C# 기본 문법
### 컨테이너(컬렉션)
- C++과 다르게 선언과 함께 메모리가 할당되지 않는다
    - EX) int[] arr = new int[5];
- C#에서는 컨테이너에 데이터를 저장할 때 Object형식의 개체로 저장하기 때문에 다양한 타입의 데이터를 넣을 수 있다
- ArrayList(List), HashTable(Dictionary), Queue, Stack 등의 컬렉션 존재

### Async, Await
- 비동기 프로그래밍을 위한 키워드(async, await)
- 수행 시간이 긴 작업을 블로킹하지 않고 비동기로 처리하여 응답성을 향상
#### async
- void / Task / Task<T>를 반환
- 메소드 내에서 await키워드를 사용할 수 있도록 만들어준다
#### await
- 비동기 작업의 흐름을 제어
- 해당 라인에서 비동기 작업이 완료될 때까지 대기, 완료되면 결과를 반환받고 다음 코드 실행
#### async, await을 사용해야 하는 이유
    I/O작업과 같이 시간이 오래 걸리는 작업을 수행하게 되면 대기 시간이 길어진다.
    이때, 비동기로 수행하여 I/O작업이 수행되는 동안 스레드가 다른 작업을 수행하여 성능을 높일 수 있다

---
# .Net Core
    - 웹 앱, 모바일 앱, 데스크톱 프로그램, 게임 프로그램, 사물인터넷 프로그램 등을 만들기 위한 오픈소스 크로스 플랫폼 개발 환경

## .Net Core의 기본 구성

### Program
- .Net Core 6부터는 Main, Startup클래스가 없어지고 Program.cs라는 하나의 파일에 기본 구성 코드가 작성됨
- 앱에서 요구하는 서비스와 미들웨어를 통한 파이프라인이 구성됨

### 미들웨어
![image](https://github.com/Kimseongtae9911/WebAPIServer/assets/84197808/5cd15e11-477b-4d19-bcfa-2a3c292afe67)

- HTTP요청에 대해 요청대리자가 존재하며, .NET Core 파이프라인의 수행을 차례대로 수행
- 대리자가 다음 대리자에 요청을 전달하지 않을 때, 이를 요청 파이프라인을 short-circuiting이라고 하며, 단락은 불필요한 작업을 방지한다. 다만, 클라이언트에 응답을 전송한 후 short-circuiting을 하면 안된다

- Use, Run, Map 세가지 메소드를 통해 등록
    1. Use: 미들웨어를 순서대로 등록하는 메소드
    2. Run: 모든 미들웨어 등록 후 마지막으로 호출하는 엔드포인트 메소드
    3. Map: 특정 URI에 대해서 분기를 나누는 메소드

- 미들웨어는 구성요소가 Configure 메소드에 추가되는 순서에 따라 호출 순서가 결정되며, HTTP요청 시에는 추가된 순선대로 호출되며, 응답 시에는 역순으로 호출된다.

#### 주요 미들웨어
1. UseExceptionHandler
    - 미들웨어에서 발생하는 예외를 잡는 기능을 하는 미들웨어
2. UseHttpsRedirection
    - 클라이언트에 Strict-Transport-Security헤더를 추가하여 HTTPS를 강제하게 만드는 미들웨어
3. UseRouting
    - HTTP요청을 일치시켜 앱의 실행 가능 앤드포인트로 디스패치하는 미들웨어
    - 요청된 URL에 따라 컨트롤러를 결정
4. UseAuthentication
   - 인증 미들웨어 활성화(클라이언트의 요청에 대해 사용자 인증을 처리, 인증된 사용자에게 접근 권한 부여)
   - 요청이 처리되기 전에 인증 정보를 확인하고, 사용자의 로그인 여부를 판단하도록 한다
5. UseAuthorization
   - 보안 리소스에 대해 권한 부여 활성화
   - 사용자의 권한에 따라 보안 리소스에 접근을 허용하거나 거부하는 작업 수행
6. UseEndPoints
   - 라우팅 및 앤드포인트 매핑을 설정하는 역할(요청이 어떤 엔드포인트에 매핑되는지 결정, 해당 엔드포인트에서 어떤 핸들러가 실행될지 지정)
   - 보통 메소드 내에 MapControllers()를 호출하여 컨트롤러의 엔드포인트를 매핑한다(직접 앤드포인트를 매핑할 수 있다)

### 라우팅
- 라우팅은 HTTP요청을 일치시켜 앱의 실행가능 앤드포인트로 디스패치하는 역할을 담당
- 클라이언트가 보낸 요청의 URL을 컨트롤러에 매핑, 컨트롤러에 [Route]특성을 사용하여 경로 지정

## 의존성 주입(Dependency Injection)
- 의존 관계의 볼륨이 클수록 유지보수가 힘든 것을 해결하기 위한 디자인 패턴
- 클래스 간의 강한 결합도를 줄이고 느슨한 결합을 이루도록 하는 것
- 코드의 품질과 유지보수성을 높일 수 있으며, 단위 테스트와 리팩토링을 하기 수월해진다

### 의존성 주입의 수명 주기
1. AddTransient
   - 서비스를 요청할 때마다 새로운 인스턴스를 생성
2. AddScope
   - 하나의 스코프에서 동일한 인스턴스를 생성, 다른 요청이 들어오면 새로운 인스턴스가 생성됨(하나의 HTTP요청에 대해)
3. AddSingleton
   - 애플리케이션 전체에서 하나의 인스턴스를 생성하고 공유

*수명주기를 선택할 때는 thread-safe동작 여부, 성능(메모리)에 따라 선택한다

---
# Redis
    -데이터를 메모리에 저장하고 조회하기 위한 고성능 key-value 저장소이다.

## Redis의 특징
````C#
1. 메모리에 데이터를 저장하기 떄문에 빠른 읽기와 쓰기가 가능하여 캐시로 활용할 때 효과적이다
2. key-value외에도 문자열, 리스트, 해시, 셋, Sorted Set 등 다양한 데이터 타입을 지원한다
3. Master-Slave 구조의 클러스터 지원
4. Publish/Subscribe모델 지원, 하나의 클라이언트가 메시지를 발행하면 다른 클라이언트는 해당 메시지를 구독하여 처리가능
5. 싱글스레드로 동작
6. 데이터 영속화 지원
````

## Redis 명령어
### String
1. SET : key에 value 저장
    - SET 'key' 'value'
2. GET : key에 해당하는 value를 가져옴
    - GET 'key'
3. DEL : key와 value 삭제
    - DEL 'key'

### List
1. LPUSH/RPUSH : 왼쪽/오른쪽에 데이터 추가
   - LPUSH '리스트이름' 'value'
2. LPOP/RPOP : 왼쪽/오른쪽에서 데이터 팝
   - LPOP '리스트이름'
3. LINDEX : 인덱스의 데이터 조회
   - LINDEX '리스트이름' '인덱스'
4. LRANGE : 특정 범위에 해당하는 데이터 조회
   - LRANGE '리스트이름' '시작인덱스' '마지막인덱스'
   - LRANGE '리스트이름' 0 -1 => 모든 요소 조회
5. LLEN : 리스트의 길이
   - LLEN '리스트이름'
6. LREM : 특정 값과 일치하는 요소 제거
   - LREM '리스트이름' '개수' 'value'
   - 리스트에서 value를 개수만큼 제거
7. LTRIM : 일부 범위를 유지하고 나머지 삭제
   - LTRIM '리스트이름' '시작인덱스' '마지막인덱스'
   - 시작인덱스부터 마지막인덱스까지의 값만 유지하고 나머지 삭제

### Set
1. SADD : 멈버 추가
   - SADD '셋이름' '멤버'
2. SMEMBERS : 모든 멤버 조회
   - SMEMBERS '셋이름'
3. SISMEMBER : 멤버가 Set에 속해 있는지 확인(0 또는 1반환)
   - SISMEMBER '셋이름' '멤버'
4. SREM : 멤버 제거
   - SREM '셋이름 '멤버'
5. SUNION : Set의 합집합
   - SUNION '셋이름1' '셋이름2'
6. SINTER : Set의 교집합
   - SINTER '셋이름1' '셋이름2'
7. SDIFF : Set의 차집합
   - SDIFF '셋이름1' '셋이름2'

### Sorted Set
- 가중치와 함께 값을 저장하여 가중치를 기준으로 데이터를 정렬하여 저장
1. ZADD : 멤버 추가
   - ZADD '셋이름' '가중치' '멤버'
2. ZRANGE : 정렬된 범위 내의 멤버 조회
   - ZRANGE '셋이름' '첫번째범위' '두번째범위'
   - ZRANGE '셋이름' 0 -1 => 모든 멤버 조회
3. ZSCORE : 멤버의 가중치 조회
   - ZSCORE '셋이름' '멤버'
4. ZINCRBY : 멤버의 가중치 증가
   - ZINCRBY '셋이름' '추가 가중치' '멤버'
5. ZREVRANK : 멤버의 역순 순위 조회
   - ZREVRANK '셋이름' '멤버'
   - 역순 순위란? 역순으로 정렬했을 때 몇번째 멤버인지
6. ZREVRANGE : 역순으로 정렬된 범위 내 멤버 조회
   - ZREVRANGE '셋이름' '첫번째범위' '두번째범위'
7. ZREM : 멤버 제거
   - ZREM '셋이름' '멤버'

## CloudStructures
- StackExchange.Redis를 기반으로 사용자가 사용하기 쉽게 기능을 추가한 라이브러리
- Cloud Structures는 Redis의 데이터 타입을 다음과 같은 클래스로 제공하며, 모든 메소드는 비동기이다(https://github.com/xin9le/CloudStructures)  
  
| Class                         | Description                             |
|-------------------------------|-----------------------------------------|
| RedisBit                      | Bits API                                |
| RedisDictionary<TKey, TValue> | Hashes API with constrained value type  |
| RedisGeo<T>                   | Geometries API                          |
| RedisHashSet<T>               | like RedisDictionary<T, bool>           |
| RedisHyperLogLog<T>           | HyperLogLog API                         |
| RedisList<T>                  | Lists API                               |
| RedisLua                      | Lua eval API                            |
| RedisSet<T>                   | Sets API                                |
| RedisSortedSet<T>             | SortedSets API                          |
| RedisString<T>                | Strings API                             |

- 각 타입별 사용 방법은 다음 깃허브 링크에 있다 (https://github.com/xin9le/CloudStructures/tree/master/src/CloudStructures/Structures)

---
# HTTP
    -웹에서 데이터를 주고받기 위해 사용되는 통신 프로토콜
    -요청은 URL을 통해 특정 웹 페이지나 리소스를 지정하고, 요청 메소드(GET, POST, PUT, DELETE 등)를 사용하여 원하는 동작을 수행한다

## 대표적인 HTTP 메소드
1. GET
    - 리소스의 데이터를 서버로부터 가져오는 메소드
    - URL에 파라미터를 포함하여 데이터를 요청할 수 있다
    - URL에 쿼리가 노출되기 때문에 중요한 데이터를 전송할 때 사용하면 안된다(ex:비밀번호)
    - 캐싱이 가능해 전송속도가 빠르며 글자 수 제한이 있다
      
2. POST
    - POST 메소드는 서버로 데이터를 전송하기 위해 사용
    - 데이터는 요청 본문(request body)에 포함되어 전송
    - 데이터가 body에 들어가기 때문에 GET에 비해 보안성이 좋다
    - 데이터 양의 제한이 없다
      
3. PUT
   - PUT 메소드는 서버에 데이터를 업데이트하기 위해 사용
   - 지정한 URL에 새로운 데이터를 전송하여 해당 데이터로 업데이트

5. DELETE
   - DELETE 메소드는 서버에서 리소스를 삭제하기 위해 사용
   - 지정한 URL에 해당하는 리소스를 삭제
   - 성공적으로 삭제되었을 때는 200을 반환

---
# 프로그래밍

---
## Program.cs
```C#
var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddTransient<IAccountDB, AccountDB>();
builder.Services.AddTransient<IItemDB, ItemDB>();
builder.Services.AddTransient<IMailboxDB, MailboxDB>();
builder.Services.AddSingleton<IMemoryDB, MemoryDB>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.UseMiddleware<VerifyUserMiddleware>();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run(configuration["ServerAddress"]);
```
````
AccountDB, ItemDB, MailboxDB등 MySqlConnection을 사용하는 클래스는 MySqlConnection객체가 thread-safe하지 않기 때문에 각 요청마다 객체를 만드는 Trasient 수명주기를 사용
MemoryDB(Redis)는 thread-safe하기 때문에 각 요청마다 객체를 만들 필요가 없으므로 Singleton 수명주기를 사용
AddControllers()-> 컨트롤러를 등록, 관리하며 라우팅을 할 수 있도록 해준다
UseRouting()-> 라우팅 미들웨어를 등록
UseMiddleware()-> 사용자 인증 커스텀 미들웨어 등록
UseEndPoints()-> 엔드포인트 미들웨어 등록., 등록된 컨트롤러를 어떤 경로에 노출할지 설정
````

---
## 컨트롤러 공통
```C#
[ApiController]
[Route("[controller]")]
public class CreateAccountController : ControllerBase
{
    readonly IAccountDB _accountDB;

    public CreateAccountController(IAccountDB accountDB)
    {
        _accountDB = accountDB;
    }

    [HttpPost]
    public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request)
    {
        var response = new CreateAccountResponse();

        var errorCode = await _accountDB.CreateAccount(request.ID, request.Password);

        response.Result = errorCode;
        return response;
    }
}
```
````
[ApiController]속성은 해당 클래스가 웹 API컨트롤러임을 알려준다
[Route("[controller]")]속성은 컨트롤러의 라우팅을 정의한다, CreateAccountController는 /CreateAccount로 라우팅된다
ControllerBase를 상속하여 웹 API컨트롤러의 기본 기능과 컨텍스트를 제공한다
[HttpPost]속성은 해당 메소드가 HTTP POST요청을 처리함을 나타낸다
````

---
## 계정 생성
```C#
[ApiController]
[Route("[controller]")]
public class CreateAccountController : ControllerBase
{
    readonly IAccountDB _accountDB;

    public CreateAccountController(IAccountDB accountDB)
    {
        _accountDB = accountDB;
    }

    [HttpPost]
    public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request)
    {
        var response = new CreateAccountResponse();

        var errorCode = await _accountDB.CreateAccount(request.ID, request.Password);

        response.Result = errorCode;
        return response;
    }
}
```
````
_accountDB 인터페이스의 CreateAccount메소드를 호출하고 결과에 따라 CreateAcountResponse객체를 반환한다
````

```C#
public async Task<ErrorCode> CreateAccount(string id, string password)
    {
        try
        {
            var saltValue = Security.GetSaltString();
            var hashedPassword = Security.HashPassword(saltValue, password);

            var count = await _queryFactory.Query("account")
                .InsertAsync(new
                {
                    ID = id,
                    SaltValue = saltValue,
                    Password = hashedPassword
                });

            if (count != 1)
            {
                return ErrorCode.CreateAccountFail;
            }

            Console.WriteLine($"[CreateAccount] ID: {id}, SaltValue: {saltValue}, Password: {hashedPassword}");
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[AccountDB.CreateAccount] ErrorCode: {nameof(ErrorCode.CreateAccountFailException)}, ID: {id}");
            return ErrorCode.CreateAccountFailException;
        }
    }
```
````
전달받은 아이디와 비밀번호를 account테이블에 등록하는 작업

비밀번호는 바로 바로 저장할 경우, 해킹의 위험이 있으므로 해싱작업을 수행하여 저장

솔트값은 지정한 문자를 조합하여 랜덤하게 생성되며, 해싱함수는 SHA256을 사용
````

---
## 로그인 및 게임 데이터 로딩
```C#
[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    readonly IAccountDB _accountDB;
    readonly IMemoryDB _memoryDB;
    readonly IItemDB _itemDB;

    public LoginController(IAccountDB accountDB, IItemDB itemDB, IMemoryDB memoryDB)
    {
        _accountDB = accountDB;
        _itemDB = itemDB;
        _memoryDB = memoryDB;
    }

    [HttpPost]
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var response = new LoginResponse();

        var errorCode = await _accountDB.Login(request.ID, request.Password);
        if(errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        var authToken = Security.GetSaltString();

        errorCode = await _memoryDB.RegisterUser(request.ID, authToken);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        (errorCode, var list) = await _itemDB.LoadItem(request.ID);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        response.Items = list;

        response.AuthToken = authToken;
        return response;
    }
}
```
````
_accountDB 인터페이스의 Login메소드를 호출하여 로그인 작업을 수행

사용자 인증을 위한 인증토큰 생성 -> 랜덤한 값을 생성
_memoryDB 인터페이스의 RegisterUser를 호출하여 인증토큰을 Redis에 저장

_itemDB 인터페이스의 LoadItem메소드를 호출하여 게임 데이터 로드

모든 작업이 수행된 이후, 인증토큰과 로딩한 item정보를 반환
````

### 로그인
```C#
public async Task<ErrorCode> Login(string id, string password)
    {
        try
        {
            var account = await _queryFactory.Query("account")
                .Where("ID", id)
                .FirstOrDefaultAsync<UserAccount>();

            if(account.Password == String.Empty)
            {
                Console.WriteLine($"[AccountDB.Login] ErrorCode: {nameof(ErrorCode.LoginFailNoAccount)}, ID: {id}");
                return ErrorCode.LoginFailNoAccount;
            }

            if(account.Password.Equals(Security.HashPassword(account.SaltValue, password)))
            {
                Console.WriteLine($"[Login] ID: {id}, Password: {nameof(account.Password)}");
                return ErrorCode.None;
            }
            else
            {
                Console.WriteLine($"[AccountDB.Login] ErrorCode: {nameof(ErrorCode.LoginFailWrongPassword)}, ID: {id}");
                return ErrorCode.LoginFailWrongPassword;
            }
        }
        catch(Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[AccountDB.Login] ErrorCode: {nameof(ErrorCode.LoginFailException)}, ID: {id}");
            return ErrorCode.LoginFailException;
        }
    }
```
````
FirstOrDefaultAsync<UserAccount>()를 통해 account테이블에서 일치하는 id의 데이터를 가져온다
이때, 일치하는 데이터가 없을 경우 null(default)를 반환

데이터를 성공적으로 가져온 경우, 전달받은 비밀번호와 데이터의 비밀번호가 일치하는지 확인
````

### 인증토큰 등록
```C#
public async Task<ErrorCode> RegisterUser(string id, string authToken)
    {
        try
        {
            var redis = new RedisString<string>(_redisConnection, id, _authTokenExpireDay);

            await redis.SetAsync(authToken);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MemoryDB.RegisterUser] ErrorCode: {nameof(ErrorCode.CreateAccountFailException)}, ID: {id}");
            return ErrorCode.LoginFailRegisterRedis;
        }

        Console.WriteLine($"[RegisterUser] ID: {id}, AuthToken: {authToken}");
        return ErrorCode.None;
    }
```
````
Redis에 인증토큰을 등록하는 메소드
RedisString객체를 만들어 _authTokenExpireDay만큼의 기간만큼 Redis에 저장

기간을 따로 지정하지 않고 null을 넣게 되면 무기한으로 저장됨
````

### 게임 데이터 로딩
```C#
public async Task<(ErrorCode, List<ItemInfo>)> LoadItem(string id)
    {
        try
        {
            var items = await _queryFactory.Query("item")
                       .Where("ID", id)
                       .GetAsync<ItemInfo>();


            Console.WriteLine($"[LoadItem] ID: {id}");
            return (ErrorCode.None, items.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[ItemDB.LoadItem] ErrorCode: {nameof(ErrorCode.LoadItemFailException)}, ID: {id}");
            return (ErrorCode.LoadItemFailException, new List<ItemInfo>());
        }

    }
```
````
item테이블로부터 일치하는 ID에 대해 아이템을 ItemInfo형태로 가져온다

ToList메소드를 통해 List<T>로 반환
````

---
## 우편함
```C#
[ApiController]
[Route("[controller]")]
public class MailboxController
{
    readonly IMailboxDB _mailboxDB;
    readonly IItemDB _itemDB;

    public MailboxController(IMailboxDB mailboxDB, IItemDB itemDB)
    {
        _mailboxDB = mailboxDB;
        _itemDB = itemDB;
    }

    [HttpPost("load")]
    public async Task<LoadMailboxResponse> LoadMailbox(LoadMailboxRequest request)
    {
        var response = new LoadMailboxResponse();

        (var errorCode, var mails) = await _mailboxDB.LoadMailbox(request.ID, request.PageNum);

        response.Result = errorCode;
        response.Mails = mails;

        return response;
    }

    [HttpPost("send")]
    public async Task<SendMailResponse> SendMail(SendMailRequest request)
    {
        var response = new SendMailResponse();

        response.Result = await _mailboxDB.SendMail(request.ID, request.Receiver, request.MailType, request.MailDetail);

        return response;
    }

    [HttpPost("recv")]
    public async Task<RecvMailResponse> RecvMail(RecvMailRequest request)
    {
        var response = new RecvMailResponse();

        (var errorCode, var mail) = await _mailboxDB.RecvMail(request.ID, request.MailboxID);

        switch((MailTypes)mail.MailType)
        {
            case MailTypes.Item:
                errorCode = await _itemDB.InsertItem(request.ID, mail.MailDetail);
                break;
            default:
                break;
        }

        response.Result = errorCode;
        response.Mail = mail;

        return response;
    }

    [HttpPost("recvAll")]
    public async Task<RecvAllMailResponse> RecvAllMail(RecvAllMailRequest request)
    {
        var response = new RecvAllMailResponse();

        (var errorCode, var mails) = await _mailboxDB.RecvAllMail(request.ID);

        foreach (var mail in mails)
        {
            switch ((MailTypes)mail.MailType)
            {
                case MailTypes.Item:
                    errorCode = await _itemDB.InsertItem(request.ID, mail.MailDetail);
                    break;
                default:
                    break;
            }
        }

        response.Result = errorCode;
        response.Mails = mails;

        return response;
    }

    [HttpPost("delete")]
    public async Task<DeleteRecvMailResponse> DeleteRecvMail(DeleteRecvMailRequest request)
    {
        var response = new DeleteRecvMailResponse();

        (var errorCode, var mails) = await _mailboxDB.DeleteRecvMail(request.ID);

        response.Result = errorCode;
        response.Mails = mails;

        return response;
    }

    [HttpPost("see")]
    public async Task<SeeUnRecvMailResponse> SeeUnRecvMail(SeeUnRecvMailRequest request)
    {
        var response = new SeeUnRecvMailResponse();

        (var errorCode, var mails) = await _mailboxDB.SeeUnRecvMail(request.ID);

        response.Result = errorCode;
        response.Mails = mails;

        return response;
    }

    [HttpPost("organize")]
    public async Task<OrganizeMailResponse> OrganizeMail(OrganizeMailRequest request)
    {
        var response = new OrganizeMailResponse();

        (var errorCode, var mails) = await _mailboxDB.OrganizeMail(request.ID, request.PageNum, request.IsAscending);

        response.Result = errorCode;
        response.Mails = mails;

        return response;
    }

    [HttpPost("update")]
    public async Task<BaseResponse> UpdateMailbox(BaseRequest request)
    {
        var response = new BaseResponse();

        var errorCode = await _mailboxDB.UpdateMailbox(request.ID);

        response.Result = errorCode;

        return response;
    }
}
```
````
우편함은 로딩, 메일 전송, 메일 받기, 전체 메일 받기, 받은 메일 삭제, 안읽은 메일만 보기, 메일 정렬 등 7가지의 기능을 구현

로딩
- 아이디와 페이지번호를 전달받아 LoadMailbox메소드 수행

메일 전송
- 아이디, 받을사람, 메일타입, 메일내용을 전달받아 SendMail메소드 수행

메일 받기
- 아이디 메일번호를 전달받아 RecvMail메소드 수행
- 받은 메일타입에 따라 메소드 수행

전체 메일 받기
- 아이디를 전달받아 RecvAllMail메소드 수행
- 받은 메일타입에 따라 메소드 수행

받은 메일 삭제
- 아이디를 전달받아 DeleteRecvMail메소드 수행

안읽은 메일만 보기
- 아이디를 전달받아 SeeUnRecvMail메소드 수행

메일 정렬
- 아이디, 페이지번호, 정렬방법을 전달받아 OrganizeMail수행

UpdateMailbox
- 우편함 테이블 업데이트
````

### 우편함 로딩
```C#
public async Task<(ErrorCode, List<MailboxInfo>)> LoadMailbox(string id, Int16 pageNum)
    {
        try
        {
            var mails = await _queryFactory.Query("mailbox")
                       .Where("UserID", id)
                       .Where("IsDeleted", false)
                       .Limit(_mailNumInPage)
                       .Offset((pageNum -1) * _mailNumInPage)
                       .GetAsync<MailboxInfo>();

            Console.WriteLine($"[LoadMailbox] ID: {id}");
            return new (ErrorCode.None, mails.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.LoadMailbox] ErrorCode: {nameof(ErrorCode.LoadMailboxException)}, ID: {id}");
            return new(ErrorCode.LoadMailboxException, new List<MailboxInfo>());
        }        
    }
```
````
Limit을 통해 mailbox테이블에서 읽어오는 데이터 수 제한
Offset을 통해 페이지번호에 따라 읽어오는 데이터 구분
MailboxInfo형태로 데이터를 읽어와 List<T>형태로 반환
````

### 메일 전송
```C#
public async Task<ErrorCode> SendMail(string sender, string receiver, Int16 mailType, Int16 mailDetail)
    {
        try
        {
            var mails = await _queryFactory.Query("mailbox")
                .InsertAsync(new
                {
                    Sender = sender,
                    UserID = receiver,
                    MailType = mailType,
                    MailDetail = mailDetail,
                    IsReceived = false,
                    IsDeleted = false,
                    ReceivedDate = DateTime.Now
                });

            Console.WriteLine($"[SendMail] Sender: {sender}, Receiver: {receiver}, MailType: {mailType}, MailDetail: {mailDetail}");
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.SendMail] ErrorCode: {nameof(ErrorCode.SendMailException)}, Sender: {sender}, Receiver: {receiver}, MailType: {mailType}, MailDetail: {mailDetail}");
            return ErrorCode.SendMailException;
        }
    }
```
````
mailbox테이블에 메일을 저장
DateTime.Now를 통해 전달받은 메일의 시간을 함께 저장한다
SqlKata를 사용하기 때문에 타입변환은 알아서 해준다

추가로 구현한다면, 메일 만료기한을 정하여 함께 저장
````

### 메일 받기
```C#
public async Task<(ErrorCode, MailboxInfo)> RecvMail(string id, Int16 mailboxID)
    {
        try
        {
            var recvMail = await _queryFactory.Query("mailbox")
                .Where("UserID", id)
                .Where("MailboxID", mailboxID)  
                .Where("IsReceived", false)
                .Where("IsDeleted", false)
                .FirstOrDefaultAsync<MailboxInfo>() ?? new MailboxInfo(-1, -1);

            if(recvMail.MailType == -1 && recvMail.MailDetail == -1)
            {
                return new(ErrorCode.NoMatchingMail, new(-1, -1));
            }

            await _queryFactory.Query("mailbox")
                .Where("UserID", id)
                .Where("MailboxID", mailboxID)
                .UpdateAsync(new { IsReceived = true });

            Console.WriteLine($"[RecvMail] ID: {id}, MailboxID: {mailboxID}, RecvMailType: {recvMail.MailType}, RecvMailDetail: {recvMail.MailDetail}");
            return new (ErrorCode.None, new ((Int16)recvMail.MailType, (Int16)recvMail.MailDetail));
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.RecvMail] ErrorCode: {nameof(ErrorCode.RecvMailException)}, ID: {id}, MailNum: {mailboxID}");
            return new (ErrorCode.RecvMailException, new (-1, -1));
        }
    }
```
````
mailbox테이블로부터 아이디와 메일번호에 따라 일치하는 데이터를 MailboxInfo형태로 반환
만약 일치하는 데이터가 없다면 MailboxInfo의 생성자를 통해 MailboxInfo객체 반환

데이터를 성공적으로 가져왔다면 해당 메일의 IsReceived값을 true로 update한다
````

### 모든 메일 받기
```C#
public async Task<(ErrorCode, List<MailboxInfo>)> RecvAllMail(string id)
    {
        try
        {
            var recvMails = await _queryFactory.Query("mailbox")
                .Where("UserID", id)
                .Where("IsReceived", false)
                .Where("IsDeleted", false)
                .GetAsync<MailboxInfo>();

            if (recvMails.Any())
            {
                await _queryFactory.Query("mailbox")
                    .Where("UserID", id)
                    .Where("IsReceived", false)
                    .UpdateAsync(new { IsReceived = true });

                Console.WriteLine($"[RecvAllMail] ID: {id}");
                return new(ErrorCode.None, recvMails.ToList());
            }
            else
            {
                Console.WriteLine($"[RecvAllMail] No mails to receive for ID: {id}");
                return new (ErrorCode.None, new List<MailboxInfo>());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.RecvAllMail] ErrorCode: {nameof(ErrorCode.RecvAllMailException)}, ID: {id}");
            return new(ErrorCode.RecvAllMailException, new List<MailboxInfo>());
        }
    }
```
````
RecvMail과 동일하게 수행하지만 여러 개의 메일을 수신하므로 List<T>로 데이터를 관리하는 것만 다르다
여러 개의 데이터를 가져오기 때문에 Any메소드를 통해 가져온 데이터가 유효한지 확인한다
````

### 받은 메일 삭제
```C#
public async Task<(ErrorCode, List<MailboxInfo>)> DeleteRecvMail(string id)
    {
        try
        {
            await _queryFactory.Query("mailbox")
                .Where("UserID", id)
                .Where("IsReceived", true)
                .Where("IsDeleted", false)
                .UpdateAsync(new { IsDeleted = true });

            Console.WriteLine($"[DeleteRecvMail] ID: {id}");

            return await LoadMailbox(id, 1);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.DeleteRecvMail] ErrorCode: {nameof(ErrorCode.DeleteRecvMailException)}, ID: {id}");
            return new(ErrorCode.DeleteRecvMailException, new List<MailboxInfo>());
        }
    }
```
````
mailbox테이블에서 아이디가 일치하는 데이터중 IsReceived가 true인 데이터에 대해서 IsDeleted를 true로 변경한다
테이블에서 지우는 것이 아니라 플래그를 변경하는 이유는 데이터를 지우는 것은 굉장히 오래 걸리는 작업이기 때문이다
데이터를 지우는 작업은 UpdateMailbox메소드에서 수행한다
````

### 안읽은 메일만 보기
```C#
public async Task<(ErrorCode, List<MailboxInfo>)> SeeUnRecvMail(string id)
    {
        try
        {
            var mails = await _queryFactory.Query("mailbox")
                       .Where("UserID", id)
                       .Where("IsReceived", false)
                       .Where("IsDeleted", false)
                       .Limit(_mailNumInPage)
                       .GetAsync<MailboxInfo>();


            Console.WriteLine($"[SeeUnRecvMail] ID: {id}");
            return new (ErrorCode.None, mails.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.SeeUnRecvMail] ErrorCode: {nameof(ErrorCode.SeeUnRecvMailException)}, ID: {id}");
            return new(ErrorCode.SeeUnRecvMailException, new List<MailboxInfo>());
        }
    }
```
````
LoadMail메소드와 동일하지만 IsReceived가 false인 데이터만 가져온다
````

### 메일 정렬
```C#
public async Task<(ErrorCode, List<MailboxInfo>)> OrganizeMail(string id, Int16 pageNum, bool isAscending)
    {
        if(isAscending)
        {
            return await LoadMailbox(id, pageNum);
        }
        else
        {
            try
            {
                var mails = await _queryFactory.Query("mailbox")
                    .Where("UserID", id)
                    .Where("IsDeleted", false)
                    .OrderByDesc("ReceivedDate")
                    .Limit(_mailNumInPage)
                    .Offset((pageNum - 1) * _mailNumInPage)
                    .GetAsync<MailboxInfo>();

                Console.WriteLine($"[OrganizeMail] ID: {id}, IsAscending: {isAscending}");
                return new(ErrorCode.None, mails.ToList());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error Msg: " + ex.Message + ", ");
                Console.WriteLine($"[MailboxDB.OrganizeMail] ErrorCode: {nameof(ErrorCode.OrganizeMailException)}, ID: {id}");
                return new(ErrorCode.OrganizeMailException, new List<MailboxInfo>());
            } 
        }
    }
```
````
isAscending값에 따라 오름차순, 내림차순으로 메일을 로드한다
오름차순인 경우는 LoadMailbox와 동일한 작업을 수행한다
내림차순의 경우에는 OrderByDesc메소드를 활용하여 메일이 등록된 시간을 기준으로 내림차순으로 데이터를 가져온다
````

### 우편함 업데이트
```C#
public async Task<ErrorCode> UpdateMailbox(string id)
    {
        try
        {
            await _queryFactory.Query("mailbox")
                .Where("IsDeleted", true)
                .DeleteAsync();

            Console.WriteLine($"[UpdateMailbox] ID: {id}");
            return ErrorCode.None;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Msg: " + ex.Message + ", ");
            Console.WriteLine($"[MailboxDB.UpdateMailbox] ErrorCode: {nameof(ErrorCode.UpdateMailboxException)}, ID: {id}");
            return ErrorCode.UpdateMailboxException;
        }
    }
```
````
데이터베이스에서 데이터를 지우는 것은 오래 걸리는 작업이다
따라서 서비스를 하는 도중에 데이터를 지우지 않고 한번에 지운다
IsDeleted가 true인 값을 모두 지운다
````

### 아이템 등록
```C#
public async Task<ErrorCode> InsertItem(string id, Int16 itemCode)
    {
        try
        {
            var compiledQuery = _compiler.Compile(_queryFactory.Query("item").AsInsert(new { ID = id, ItemCode = itemCode, Count = 1 }));
            var onDuplicatedKeySql = compiledQuery.Sql + " ON DUPLICATE KEY UPDATE Count=Count+1";

            await _dbConnection.ExecuteAsync(onDuplicatedKeySql, compiledQuery.NamedBindings);

            Console.WriteLine($"[InsertItem] ID: {id}, ItemCode: {itemCode}");
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[ItemDB.InsertItem] ErrorCode: {nameof(ErrorCode.InsertItemFailException)}, ID: {id}, ItemCode: {itemCode}");
            return ErrorCode.InsertItemFailException;
        }
    }
```
````
받은 메일타입이 Item에 해당하는 경우, item테이블에 데이터를 넣는데, 그를 수행하는 메소드이다
SqlKata에는 MySql의 ON DUPLICATE KEY를 지원하는 메소드가 없다
따라서 SqlKata의 raw쿼리 기능을 활용하여 쿼리문을 만들어 사용한다

중복된 키가 있을 경우 Count값을 증가시키고 없을 경우에는 Count를 1로 하여 테이블에 삽입한다
````

---
## 유저인증
```C#
public class VerifyUserMiddleware : IMiddleware
{
    private readonly RequestDelegate _next;
    readonly IMemoryDB _memoryDB;

    public VerifyUserMiddleware(RequestDelegate next, IMemoryDB memoryDB)
    {
        _next = next;
        _memoryDB = memoryDB;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if(context.Request.Path.StartsWithSegments("/CreateAccout") || context.Request.Path.StartsWithSegments("/Login"))
        {
            await _next(context);
            return;
        }

        try
        {
            context.Request.EnableBuffering();

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                var requestBody = await reader.ReadToEndAsync();

                if(string.IsNullOrEmpty(requestBody))
                {
                    Console.Write($"[VerifyUserMiddleware]: Invalid Request Body");
                    return;
                }


                var jsonDocument = JsonDocument.Parse(requestBody);

                (var userID, var authToken) = ExtractIDAndAuthToken(jsonDocument);

                if(userID == null)
                {
                    return;
                }

                (var errorCode, var registerdToken) = await _memoryDB.GetAuthToken(userID);
                if (errorCode != ErrorCode.None)
                {
                    Console.Write($"[VerifyUserMiddleware]: No Client Info In Redis");
                    return;
                }
                else if (registerdToken == authToken)
                {
                    if(ErrorCode.None != await _memoryDB.LockUserRequest(userID, authToken))
                    {
                        return;
                    }

                    context.Request.Body.Position = 0;
                    await _next(context);

                    await _memoryDB.UnlockUserRequest(userID, authToken);
                    return;
                }
            }

            Console.WriteLine($"[VerifyUserMiddleware]: AuthToken Not Valid");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message);
            Console.WriteLine($"[VerifyUserMiddleware] ErrorCode: {nameof(ErrorCode.UnhandleException)}");
        }
        
    }

    (string userID, string authToken) ExtractIDAndAuthToken(JsonDocument document)
    {
        try
        {
            var userID = document.RootElement.GetProperty("ID").GetString();
            var authToken = document.RootElement.GetProperty("AuthToken").GetString();

            if(string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(authToken))
            {
                Console.WriteLine("[VerifyUserMiddleware]: ID or AuthToken is null");
                return (string.Empty, string.Empty);
            }

            return (userID, authToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Msg: " + ex.Message);
            return (string.Empty, string.Empty);
        }
    }
}
```
````
인증토큰을 기반으로 로그인한 사용자와 동일한 사용자가 요청을 보냈는지 확인하는 커스텀 미들웨어

요청 URL을 통해 계정생성과 로그인의 요청의 경우 해당 미들웨어를 거치지 않고 다음 미들웨어로 short-circuiting한다
Request body에서 아이디와 인증토큰을 추출하여 Redis에 등록된 인증토큰과 일치하는지 확인한다
이후 스트림 위치를 초기로 돌리고 다음 미들웨어로 넘어간다

클라이언트에서 요청을 동시에 2개 이상을 보낼 수 있는데, 같은 클라이언트에 대해 동시에 2개의 요청을 처리하게 되면
데이터베이스의 데이터가 의도하지 않은 방향으로 결과가 나올 수 있다.
따라서 Redis를 활용하여 락을 걸어 동시에 2개 이상의 요청이 처리되지 않도록 한다

````

---
## Redis를 활용한 Lock
```C#
public async Task<ErrorCode> LockUserRequest(string id, string authToken)
    {
        try
        {
            var redis = new RedisString<string>(_redisConnection, id + authToken, _lockExpireSecond);

            if (false == await redis.SetAsync(id, _lockExpireSecond, StackExchange.Redis.When.NotExists))
            {
                Console.WriteLine($"[MemoryDB.LockUserRequest] ErrorCode: {nameof(ErrorCode.LockUserRequestFail)}, ID: {id}, AuthToken: {authToken}");
                return ErrorCode.LockUserRequestFail;
            }
            return ErrorCode.None;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Msg: " + ex.Message + ", ");
            Console.WriteLine($"[MemoryDB.LockUserRequest] ErrorCode: {nameof(ErrorCode.LockUserRequestFailException)}, ID: {id}, AuthToken: {authToken}");
            return ErrorCode.LockUserRequestFailException;
        }
    }

    public async Task<ErrorCode> UnlockUserRequest(string id, string authToken)
    {
        try
        {
            var redis = new RedisString<string>(_redisConnection, id + authToken, null);

            if(false == await redis.DeleteAsync())
            {
                Console.WriteLine($"[MemoryDB.UnlockUserRequest] ErrorCode: {nameof(ErrorCode.UnlockUserRequestFail)}, ID: {id}, AuthToken: {authToken}");
                return ErrorCode.UnlockUserRequestFail;
            }
            return ErrorCode.None;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Msg: " + ex.Message + ", ");
            Console.WriteLine($"[MemoryDB.UnlockUserRequest] ErrorCode: {nameof(ErrorCode.UnlockUserRequestFailException)}, ID: {id}, AuthToken: {authToken}");
            return ErrorCode.UnlockUserRequestFailException;
        }
    }
```
````
LockUserRequest메소드를 통해 아이디와 인증토큰을 키값으로 하여 Redis에 등록을 하는데,
이때 StackExchange.Redis.When.NotExists플래그를 이용하여 해당 키값이 없을 때만 등록하도록 한다

UnlockUserRequest메소드를 통해 LockUserRequest메소드를 이용하여 등록했던 키를 삭제하는 작업을 수행한다
````
