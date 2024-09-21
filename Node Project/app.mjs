// app.mjs
import express from 'express';
import morgan from 'morgan';
import mobilerouter from './routes/mobileRouter.mjs';

const app = express();

app.use(morgan('dev'));
app.use(express.json());  // To parse JSON bodies
//app.use(express.urlencoded({ extended: true })); // To parse URL-encoded bodies
app.use(express.static('./public'))

app.use('/', mobilerouter);

app.all('*',(req,res,next)=>{
    const err= new Error(`Cant find ${req.orignalUrl} on the server`);
})
app.use((error,req,res,next) => {
    error.statusCode=error.statusCode|| '500';
    error.status=error.status||'error'

    res.status(erro.statusCode).json({
        status:error.statusCode,
        message:error.message
    })

})
export default app;
