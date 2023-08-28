# 목차
1. 이론
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
2. 프로그래밍
   1. [계정 생성](#계정-생성)
   2. [로그인](#로그인)
   3. [게임 데이터 로딩](#게임-데이터-로딩)
   4. [우편함](#우편함)

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
