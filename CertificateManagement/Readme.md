# 🔧 Étapes pour générer un certificat utilisateur de test

## 1. Générer une autorité de certification (CA) locale

```
# Clé privée de la CA
openssl genrsa -out rootCA.key 4096

# Certificat auto-signé de la CA (valable 10 ans)
openssl req -x509 -new -nodes -key rootCA.key -sha256 -days 3650 -out rootCA.crt \
  -subj "/C=FR/ST=AURA/L=GRENOBLE/O=MYORG/OU=R&D/CN=myorg"
```

👉 Le fichier rootCA.crt sera importé comme autorité de confiance dans Keycloak (et éventuellement dans ton navigateur, si ce n’est pas déjà reconnu).

## 2. Générer la clé et CSR (Certificate Signing Request) pour l’utilisateur

```
# Clé privée utilisateur
openssl genrsa -out anthony.key 2048

# CSR utilisateur
openssl req -new -key anthony.key -out anthony.csr \
  -subj "/C=FR/ST=AURA/L=GRENOBLE/O=MYORG/OU=R&D/CN=anthony/emailAddress=anthony.coudene@technidata-web.com"
```

## 3. Signer le certificat utilisateur avec ta CA

```
openssl x509 -req -in anthony.csr -CA rootCA.crt -CAkey rootCA.key -CAcreateserial \
  -out anthony.crt -days 365 -sha256
```

👉 Tu as maintenant anthony.crt signé par ta CA locale.

## 4. Créer un fichier PFX pour import dans le navigateur

Les navigateurs veulent souvent un PKCS#12 (.p12 ou .pfx) contenant le certificat et la clé privée :

```
openssl pkcs12 -export -out anthony.pfx -inkey anthony.key -in anthony.crt -certfile rootCA.crt
```

ou P12

```
openssl pkcs12 -export -out anthony.p12 -inkey anthony.key -in anthony.crt -certfile rootCA.crt
```

Il te sera demandé un mot de passe → saisis-en un simple (ex. test123).

## 5. Générer le Host Certificate (https)


```
openssl req -new -newkey rsa:4096 -keyout localhost.key -out localhost.csr -nodes -subj "/C=FR/ST=AURA/L=GRENOBLE/O=MYORG/OU=
R&D/CN=localhost/emailAddress=anthony.coudene@technidata-web.com"
```

## 6. Signer le certificat localhost avec ta CA

Ecrire un fichier localhost.ext ou selon le nom du host :

```
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
subjectAltName=@alt_names
[alt_names]
DNS.1=localhost
```

Puis signer le certificat :

```
openssl x509 -req -CA rootCA.crt -CAkey rootCA.key -in localhost.csr -out localhost.crt -days 365 -CAcreateserial -extfile localhost.ext
```

## 7. Créer un fichier pem pour le host certificate

```
openssl x509 -in localhost.crt -out localhost-crt.pem -outform PEM
```

Puis 

```
openssl rsa -in  localhost.key -out localhost-key.pem
```

## 8. Créer un truststore pour le host certificate

```
keytool -import -alias localhost -file rootCA.crt -keystore truststore.jks -storepass password
```

## 9. Importer le certificat dans ton navigateur

Chrome / Edge / Brave :

Paramètres → Confidentialité et sécurité → Sécurité → Gérer les certificats → Onglet Personnel → Importer user.pfx.

Firefox :

Préférences → Vie privée et sécurité → Certificats → Afficher les certificats → Onglet Vos certificats → Importer user.pfx.

Safari (macOS/iOS) : double-clique sur user.pfx, il sera ajouté au trousseau d’accès.

## 10. Importer la CA dans Keycloak

Dans Keycloak, configuration TLS mutualisé → tu dois indiquer que ca.crt est une CA de confiance pour vérifier les certificats utilisateurs.

Selon ton déploiement :

Si c’est Keycloak direct → configurer truststore côté Quarkus.

Si c’est via un proxy (NGINX/Traefik) → configure le proxy pour faire confiance à ca.crt et exiger un certificat client.

# Tests

## Direct authentication

https://localhost:8443/realms/mytenant/protocol/openid-connect/auth?client_id=authenticationCertificateApp&response_type=code&redirect_uri=https://mytenant.localhost.com:5002

# Références

https://medium.com/@sangeethapl.sai/keycloak-x509-certificate-based-login-e9101f4a9922
