# Kubernetes

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