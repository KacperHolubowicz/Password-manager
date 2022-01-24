CREATE TABLE IF NOT EXISTS User(
  ID integer primary key,
  Username text not null unique,
  Login text not null unique,
  Email text not null unique,
  Password blob not null,
  MasterPassword blob not null,
  Salt blob not null
);

CREATE TABLE IF NOT EXISTS ServicePassword(
  ID integer primary key,
  UserID integer,
  Description text not null,
  Password blob not null,
  IV blob not null,
  foreign key(UserID) references User(ID)
);

CREATE TABLE IF NOT EXISTS Device(
  ID integer primary key,
  UserID integer,
  DeviceType text not null,
  OperatingSystem text not null,
  Browser text not null,
  IpAddress text not null,
  foreign key(UserID) references User(ID)
);

CREATE TABLE IF NOT EXISTS LoginAttempt(
  ID integer primary key,
  IpAddress text not null,
  Timestamp text not null,
  Successful integer not null
);

CREATE Table IF NOT EXISTS Blocking(
  ID integer primary key,
  IpAddress text not null,
  Timestamp text not null,
  BlockedUntil text not null
);
