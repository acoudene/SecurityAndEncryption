# Environment Variables as secrets

- Define this variable only for IIS process or application.
- This variable should not be available to system level or other processes.
- Logs should not contain this variable.

# Docker Swarm approach for secrets

## Secret cration

`docker secret create my-secret -`

## Secret declaration in Yaml

```yaml
secrets:
  my-secret:
    external: true
```

## Secret usage in Yaml

```yaml 
services:
  my-service:
    secrets:
      - my-secret
```

## Secret location

`/run/secrets/my-secret`

# Kubernetes approach for secrets

## Yaml

Get secret as Environment Variable:

```yaml
env:
  - name: AppConfig__Message
    valueFrom:
      secretKeyRef:
        name: my-secret
        key: secret-message
```

## Commands

### Logs

`kubectl logs debian-pod` ou `kubectl logs debian-pod -c nom-du-conteneur`

### Delete Pod

`kubectl delete pod debian-pod`

### Create Pod from Yaml

`kubectl apply -f ./debian-pod.yaml`

### Create Secret

`kubectl create secret generic my-secret --from-literal=secret-message=Message secret`

### Get Secret

`kubectl get secret my-secret -o jsonpath='{.data.secret-message}' | base64 –decode`

### Update Secret

`echo -n Message secret|base64`

### Update Secret with Patch

`kubectl patch secret my-secret -p '{"data": {"secret-message": "YW50aG9ueQ=="}}'`

### Get Secret in YAML Format

`kubectl get secret my-secret -o yaml`