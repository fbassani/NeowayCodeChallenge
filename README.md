Feito com .NET Core, é necessário instalar o sdk: https://www.microsoft.com/net/download/linux


### importação do arquivo de exemplo no mongodb (bash)

```
tr ";" "," < q1_catalog.csv | mongoimport -d yawoen -c companies --type csv --headerline
```


### transformação dos ceps para string

```
db.companies.find().forEach(function(x) { db.companies.update({_id:x._id}, {$set: {addressZip: ""+x.addressZip}}); })
```

### índice no nome da empresa

```
db.companies.createIndex({name: 1})
```

