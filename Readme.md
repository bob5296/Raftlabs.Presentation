//TODO
// Can add directory.props.packages for centralized pacakage management
// CQRS or IMediator for loose coupling between Presentation and Application
// Key Vault for configuration Management




// Run project
// set Raftlabs.presentation as startup project and F5

// Get user by id curl
curl --location 'https://localhost:44397/users/13' \
--header 'accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8' \
--header 'accept-language: en-US,en;q=0.8' \
--header 'cache-control: max-age=0' \
--header 'priority: u=0, i' \
--header 'referer: https://www.google.com/' \
--header 'sec-ch-ua: "Brave";v="137", "Chromium";v="137", "Not/A)Brand";v="24"' \
--header 'sec-ch-ua-mobile: ?0' \
--header 'sec-ch-ua-platform: "Windows"' \
--header 'sec-fetch-dest: document' \
--header 'sec-fetch-mode: navigate' \
--header 'sec-fetch-site: cross-site' \
--header 'sec-fetch-user: ?1' \
--header 'sec-gpc: 1' \
--header 'upgrade-insecure-requests: 1' \
--header 'user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36' \
--header 'x-api-key: reqres-free-v1'


// Get all users curl
curl --location 'https://localhost:44397/users?pagenumber=1' \
--header 'accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8' \
--header 'accept-language: en-US,en;q=0.8' \
--header 'cache-control: max-age=0' \
--header 'priority: u=0, i' \
--header 'referer: https://www.google.com/' \
--header 'sec-ch-ua: "Brave";v="137", "Chromium";v="137", "Not/A)Brand";v="24"' \
--header 'sec-ch-ua-mobile: ?0' \
--header 'sec-ch-ua-platform: "Windows"' \
--header 'sec-fetch-dest: document' \
--header 'sec-fetch-mode: navigate' \
--header 'sec-fetch-site: cross-site' \
--header 'sec-fetch-user: ?1' \
--header 'sec-gpc: 1' \
--header 'upgrade-insecure-requests: 1' \
--header 'user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36' \
--header 'x-api-key: reqres-free-v1'