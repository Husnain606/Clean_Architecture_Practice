import express from 'express';
import { createPhone, getAllPhones, getPhone, updatePhone, deletePhone } from '../Controler/mobilesControler.mjs';

const router = express.Router();

router
    .route('/mobiles')
    .get(getAllPhones)
    .post(createPhone);

router
    .route('/mobiles/:id')
    .get(getPhone)
    .patch(updatePhone)
    .delete(deletePhone);

export default router;
