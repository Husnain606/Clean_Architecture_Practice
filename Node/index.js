const http  = require("http")
const myServer = http.createServer((req,res)   =>  {
    console.log(req.url)
    res.end("Hello Node")
})
myServer.listen(3000,()=> console.log("Server Started "));