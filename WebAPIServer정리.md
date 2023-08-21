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
5. UseAuthorization
6. UseEndPoints