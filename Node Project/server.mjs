import mongoose from 'mongoose';
import dotenv from 'dotenv';
dotenv.config({ path: './config.env' });

import app from './app.mjs';

const port = process.env.PORT || 4000;

const connectionString = process.env.NODE_ENV === 'development'
  ? process.env.local_con_str
  : process.env.con_str;

mongoose.connect(connectionString)
  .then(() => console.log('DB Connection Successful'))
  .catch(err => console.error('DB Connection Error:', err));

app.listen(port, () => {
    console.log(`Server has started on port ${port}`);
});
