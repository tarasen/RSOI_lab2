# RSOI_lab2
REST service API. Picture gallery with ServiceStack, OrmLite and SqlServer.

Available routes:
```
GET			/pictures
GET			/pictures/{id}
POST		/picures
PUT			/pictures/{id}
DELETE		/pictures/{id}

GET			/artists
GET			/artists/{id}
GET			/artists/{id}/pictures
POST		/artists
PUT			/artists/{id}
DELETE		/artists/{id}

GET 		/profile

GET 		/auth
POST 		/token
```

OAuth query:
`/auth?client_id={YOUR_CLIENT_ID}&redirect_uri={YOUR_REDIRECT_URL}&state={SOME_STATE}`

Token request must contain next parameters:
* ClientId
* SecretKey
* RederectUri
* Code

Code property is obtaining after auth stage.
