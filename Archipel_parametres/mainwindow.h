#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <iostream>
#include <QMainWindow>
#include <QLabel>
#include <QDebug>
#include <QFile>
#include <QLineEdit>
#include <QXmlStreamWriter>
#include <QMessageBox>

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    bool verify();

private:
    Ui::MainWindow *ui;

public slots:
    void applyChanges();
};

#endif // MAINWINDOW_H
