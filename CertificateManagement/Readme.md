# 🔧 Étapes pour générer un certificat utilisateur de test

## 1. Générer une autorité de certification (CA) locale

```
# Clé privée de la CA
openssl genrsa -out myca.key 4096

# Certificat auto-signé de la CA (valable 10 ans)
openssl req -x509 -new -nodes -key myca.key -sha256 -days 3650 -out myca.crt \
  -subj "/C=FR/ST=Auvergne Rhône-Alpes/L=Grenoble/O=MyCA/OU=IT/CN=My Root CA"
```

👉 Le fichier ca.crt sera importé comme autorité de confiance dans Keycloak (et éventuellement dans ton navigateur, si ce n’est pas déjà reconnu).

## 2. Générer la clé et CSR (Certificate Signing Request) pour l’utilisateur

```
# Clé privée utilisateur
openssl genrsa -out anthony.key 2048

# CSR utilisateur
openssl req -new -key anthony.key -out anthony.csr \
  -subj "/C=FR/ST=Auvergne Rhône-Alpes/L=Grenoble/O=MyOrg/OU=Dev/CN=anthony/emailAddress=anthony.coudene@technidata-web.com"
```

## 3. Signer le certificat utilisateur avec ta CA

```
openssl x509 -req -in anthony.csr -CA myca.crt -CAkey myca.key -CAcreateserial \
  -out anthony.crt -days 365 -sha256
```

👉 Tu as maintenant user.crt signé par ta CA locale.

## 4. Créer un fichier PFX pour import dans le navigateur

Les navigateurs veulent souvent un PKCS#12 (.p12 ou .pfx) contenant le certificat et la clé privée :

```
openssl pkcs12 -export -out anthony.pfx -inkey anthony.key -in anthony.crt -certfile myca.crt
```

Il te sera demandé un mot de passe → saisis-en un simple (ex. test123).

## 5. Importer le certificat dans ton navigateur

Chrome / Edge / Brave :

Paramètres → Confidentialité et sécurité → Sécurité → Gérer les certificats → Onglet Personnel → Importer user.pfx.

Firefox :

Préférences → Vie privée et sécurité → Certificats → Afficher les certificats → Onglet Vos certificats → Importer user.pfx.

Safari (macOS/iOS) : double-clique sur user.pfx, il sera ajouté au trousseau d’accès.

## 6. Importer la CA dans Keycloak

Dans Keycloak, configuration TLS mutualisé → tu dois indiquer que ca.crt est une CA de confiance pour vérifier les certificats utilisateurs.

Selon ton déploiement :

Si c’est Keycloak direct → configurer truststore côté Quarkus.

Si c’est via un proxy (NGINX/Traefik) → configure le proxy pour faire confiance à ca.crt et exiger un certificat client.