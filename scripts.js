// task 1
db.books.insertMany([
    {
        name: "Hobbit",
        author: "Tolkien",
        count: NumberInt(5),
        gener: ["fantasy"],
        year: NumberInt(2014)
    },
    {
        name: "Lord of the rings",
        author: "Tolkien",
        count: NumberInt(3),
        gener: ["fantasy"],
        year: NumberInt(2015)
    },
    {
        name: "Kolobok",
        count: NumberInt(10),
        gener: ["kids"],
        year: NumberInt(2000)
    },
    {
        name: "Repka",
        count: NumberInt(11),
        gener: ["kids"],
        year: NumberInt(2000)
    },
    {
        name: "Dyadya Stiopa",
        author: "Mihalkov",
        count: NumberInt(1),
        gener: ["kids"],
        year: NumberInt(2001)
    }
]);

// task 2
db.books
    .find({count: {$gt: 1}}, {name: 1, _id: 0})
    .sort({name: 1})
    .limit(3);

// task 3
db.books.find().sort({count: -1}).limit(1); // for MAX
db.books.find().sort({count: +1}).limit(1); // for MIN

// task 4
db.books.aggregate( [ { $group : { _id : "$author" } } ] );

// task 5
db.books.find({author: {$exists: false}})

// task 6
db.books.update({}, {$inc: {count: 1}}, {multi: true});

// task 7
db.books.update(
    { gener: { $elemMatch: { $eq: "fantasy" } } },
    { $addToSet: { gener: "favority" } },
    { multi: true }
);

// task 8
db.books.remove({count: {$lt: 3}});

// task 9
db.books.remove({});
